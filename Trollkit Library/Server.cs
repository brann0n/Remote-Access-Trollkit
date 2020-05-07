using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Trollkit_Library.Models;
using Trollkit_Library.Modules;

namespace Trollkit_Library
{
	public class Server
	{
		private IPAddress ip;
		private int dataSize;
		private Socket serverSocket;
		private bool acceptIncomingConnections;
		private uint clientCount = 0;
		private List<DataBufferModel> Buffers;

		/// <summary>
		/// Contains all connected clients indexed
		/// by their socket.
		/// </summary>
		private Dictionary<Socket, Client> clients;

		//delegates
		public delegate void ConnectionEventHandler(Client c);
		public delegate void ConnectionBlockedEventHandler(IPEndPoint endPoint);
		public delegate void ClientMessageReceivedHandler(Client c, TransferCommandObject model, DataByteType type);

		/// <summary>
		/// Occures when a client is connected.
		/// </summary>
		public event ConnectionEventHandler ClientConnected;

		/// <summary>
		/// Occures when a client is disconnected.
		/// </summary>
		public event ConnectionEventHandler ClientDisconnected;

		/// <summary>
		/// Occures when an incoming connection is blocked.
		/// </summary>
		public event ConnectionBlockedEventHandler ConnectionBlocked;

		/// <summary>
		/// Occures when a message is received by the server.
		/// </summary>
		public event ClientMessageReceivedHandler MessageReceived;

		public enum DataByteType
		{
			Response = 0x1A,
			Command = 0x1B,
			Data = 0x1C
		}

		public Server(IPAddress ip)
		{
			this.ip = ip;
			dataSize = 2048;
			clients = new Dictionary<Socket, Client>();
			acceptIncomingConnections = true;
			serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			Buffers = new List<DataBufferModel>();
		}

		/// <summary>
		/// Starts the server.
		/// </summary>
		public void Start()
		{
			serverSocket.Bind(new IPEndPoint(ip, 6969));
			serverSocket.Listen(0);
			serverSocket.BeginAccept(new AsyncCallback(HandleIncomingConnection), serverSocket);
		}

		/// <summary>
		/// Stops the server
		/// </summary>
		public void Stop()
		{
			serverSocket.Close();
		}

		/// <summary>
		/// Async function that handles the incoming connection
		/// </summary>
		/// <param name="result"></param>
		private void HandleIncomingConnection(IAsyncResult result)
		{
			try
			{
				Socket oldSocket = (Socket)result.AsyncState;
				Socket newSocket = oldSocket.EndAccept(result);

				uint clientId = clientCount++;
				Client client = new Client(clientId, (IPEndPoint)newSocket.RemoteEndPoint);
				clients.Add(newSocket, client);

				ClientConnected(client);

				//TODO: send data to client for verification

				serverSocket.BeginAccept(new AsyncCallback(HandleIncomingConnection), serverSocket);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		/// <summary>
		/// The async callback of receivedata
		/// </summary>
		/// <param name="result"></param>
		private void ReceiveData(IAsyncResult result)
		{
			try
			{
				Socket clientSocket = (Socket)result.AsyncState;
				Client client = GetClientBySocket(clientSocket);
				int bytesReceived = clientSocket.EndReceive(result);
				DataByteType type = (DataByteType)client.Data[0];
				if(bytesReceived == 0)
				{
					CloseSocket(clientSocket);
					serverSocket.BeginAccept(new AsyncCallback(HandleIncomingConnection), serverSocket);
				}
				else if(Enum.IsDefined(typeof(DataByteType), client.Data[0]))
				{
					int length = client.Data[1];
					int series = client.Data[2];
					Guid guid = new Guid(client.Data.SubArray(3, 16));

					DataBufferModel buffer = Buffers.FirstOrDefault(n => n.DataId == guid);
					if(buffer != null)
					{
						buffer.BufferedData.Add(series, client.Data.SubArray(19, 2028));
						buffer.LatestSeries = series;
					}
					else
					{
						buffer = new DataBufferModel();
						buffer.BufferedData.Add(series, client.Data.SubArray(19, 2028));
						buffer.DataId = guid;
						buffer.SeriesLength = length;
						buffer.LatestSeries = series;
						Buffers.Add(buffer);
					}
					Console.WriteLine($"Received data with id: {guid.ToString()}");
					
					if(buffer.BufferedData.Count == buffer.SeriesLength)
					{				
						if (HandleIncomingData(ClientServerPipeline.BufferDeserialize(buffer), client, type))
						{
							//todo: remove id from buffer
						}
					}
				}

			}
			catch (SocketException e)
			{
				Socket clientSocket = (Socket)result.AsyncState;
				Client client = GetClientBySocket(clientSocket);
				KickClient(client);
			}
			catch (Exception e)
			{
				Console.WriteLine("Error occured: " + e.Message.Substring(0, 20));
			}
		}

		/// <summary>
		/// Function that handles the converted data
		/// </summary>
		/// <param name="dObj"></param>
		/// <param name="client"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		private bool HandleIncomingData(TransferCommandObject dObj, Client client, DataByteType type)
		{
			MessageReceived?.Invoke(client, dObj, type);

			return true;
		}

		/// <summary>
		/// Sends a text message to the specified
		/// socket.
		/// </summary>
		/// <param name="s">The socket.</param>
		/// <param name="message">The message.</param>
		public void SendDataObjectToSocket(DataByteType type, Socket s, DataBufferModel message)
		{
			Console.WriteLine("Sending data with id: " + message.DataId.ToString());

			foreach (KeyValuePair<int, byte[]> item in message.BufferedData)
			{
				byte[] sendArray = new byte[] { (byte)type, (byte)message.SeriesLength, (byte)item.Key };
				sendArray = sendArray.Concat(message.DataId.ToByteArray()).Concat(item.Value).ToArray();
				SendBytesToSocket(s, sendArray);
			}
		}

		/// <summary>
		/// Sends bytes to the specified socket.
		/// </summary>
		/// <param name="s">the socket to send data to</param>
		/// <param name="data">the bytes to send</param>
		private void SendBytesToSocket(Socket s, byte[] data)
		{
			s.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendData), s);
		}

		private void SendData(IAsyncResult result)
		{
			try
			{
				Socket clientSocket = (Socket)result.AsyncState;
				Client client = GetClientBySocket(clientSocket);
				clientSocket.EndSend(result);


				clientSocket.BeginReceive(client.Data, 0, dataSize, SocketFlags.None, new AsyncCallback(ReceiveData), clientSocket);
			}
			catch (Exception e)
			{
				Console.WriteLine("Error occured: " + e.Message.Substring(0, 20));
			}
		}

		/// <summary>
		/// Gets the client by socket.
		/// </summary>
		/// <param name="clientSocket">The client's socket.</param>
		/// <returns>If the socket is found, the client instance
		/// is returned; otherwise null is returned.</returns>
		private Client GetClientBySocket(Socket clientSocket)
		{
			if (!clients.TryGetValue(clientSocket, out Client c))
			{
				c = null;
			}

			return c;
		}

		/// <summary>
		/// Gets the socket by client.
		/// </summary>
		/// <param name="client">The client instance.</param>
		/// <returns>If the client is found, the socket is
		/// returned; otherwise null is returned.</returns>
		public Socket GetSocketByClient(Client client)
		{
			return clients.FirstOrDefault(x => x.Value.GetClientID() == client.GetClientID()).Key;
		}

		/// <summary>
		/// Kicks the specified client from the server.
		/// </summary>
		/// <param name="client">The client.</param>
		public void KickClient(Client client)
		{
			Socket s = GetSocketByClient(client);
			if (s != null)
			{
				CloseSocket(s);
				ClientDisconnected(client);
			}
		}

		/// <summary>
		/// Closes the socket and removes the client from
		/// the clients list.
		/// </summary>
		/// <param name="clientSocket">The client socket.</param>
		private void CloseSocket(Socket socket)
		{
			socket.Close();
			clients.Remove(socket);
		}

		/// <summary>
		/// Send a buffer model to all clients
		/// </summary>
		/// <param name="message"></param>
		public void SendDataObjectToAll(DataBufferModel message)
		{
			foreach(Socket s in clients.Keys)
			{
				try
				{
					SendDataObjectToSocket(DataByteType.Data, s, message);
				}
				catch { }
			}
		}

		/// <summary>
		/// Returns a list of Connected clients
		/// </summary>
		/// <returns></returns>
		public List<Client> GetClients()
		{
			return clients.Values.ToList();
		}
	}


}
