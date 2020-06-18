using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Trollkit_Library.Models;
using Trollkit_Library.Modules;
using Trollkit_Library.ClientModules;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Trollkit_Library.ViewModels;

namespace Trollkit_Library.ServerModules
{
	public class Server
	{
		private IPAddress ip;
		private readonly int dataSize;
		private readonly Socket serverSocket;
		private uint clientCount = 0;
		private readonly List<DataBufferModel> Buffers;
		private Client selectedClient;
		private bool allClientsSelected;

		/// <summary>
		/// Contains all connected clients indexed
		/// by their socket.
		/// </summary>
		private Dictionary<Socket, Client> clients;

		public List<Client> Clients
		{
			get
			{
				return clients.Values.ToList();
			}
		}

		public Client SelectedClient
		{
			get
			{
				return selectedClient;
			}
			set
			{
				selectedClient = value;
				OnPropertyChanged("SelectedClient");
			}
		}

		public bool AllClientsSelected
		{
			get
			{
				return allClientsSelected;
			}
			set
			{
				allClientsSelected = value;
				OnPropertyChanged("AllClientsSelected");
			}
		}

		public bool ClientsAvailable { get { return Clients.Count != 0; } }

		//delegates
		public delegate void ConnectionEventHandler(Client c);
		public delegate void ConnectionBlockedEventHandler(IPEndPoint endPoint);
		public delegate void ClientMessageReceivedHandler(Client c, TransferCommandObject model, DataByteType type);
		public delegate void ServerPropertyChangedHandeler(string Property);

		/// <summary>
		/// Occures when a client is connected.
		/// </summary>
		public event ConnectionEventHandler ClientConnected;

		/// <summary>
		/// Occures when a client is disconnected.
		/// </summary>
		public event ConnectionEventHandler ClientDisconnected;

		/// <summary>
		/// Occures when a message is received by the server.
		/// </summary>
		public event ClientMessageReceivedHandler MessageReceived;

		/// <summary>
		/// Occures when we need to update a property in the UI through the static reference.
		/// </summary>
		public event ServerPropertyChangedHandeler OnPropertyChanged;

		/// <summary>
		/// Enum that holds the send, response and data types for the data framework
		/// </summary>
		public enum DataByteType
		{
			Response = 0x1A,
			Command = 0x1B,
			Data = 0x1C
		}

		public Server(IPAddress ip)
		{
			this.ip = ip;
			dataSize = SharedProperties.DataSize;
			clients = new Dictionary<Socket, Client>();
			serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			Buffers = new List<DataBufferModel>();
			allClientsSelected = false;
		}

		/// <summary>
		/// Starts the server.
		/// </summary>
		public void Start()
		{
			serverSocket.Bind(new IPEndPoint(ip, SharedProperties.MainPort));
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
				clients.FirstOrDefault(m => m.Key == newSocket).Value.SetDataItem("test", "DEBUG_TEST_ITEM2");

				//call the client connected event, this is then passed up into the ServerViewModel.
				ClientConnected(client);

				//tell the client to start sending back its client info.
				TransferCommandObject returnObject = new TransferCommandObject { Command = "GetClientInfo", Handler = "SystemInfo"};
				SendDataObjectToSocket(DataByteType.Command, newSocket, ClientServerPipeline.BufferSerialize(returnObject));

				serverSocket.BeginAccept(new AsyncCallback(HandleIncomingConnection), serverSocket);
			}
			catch (Exception e)
			{
				BConsole.WriteLine(e.Message);
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
				DataByteType type = (DataByteType)client.Data[SharedProperties.TypeByte];
				if(bytesReceived == 0)
				{
					CloseSocket(clientSocket);
					serverSocket.BeginAccept(new AsyncCallback(HandleIncomingConnection), serverSocket);
				}
				else if(Enum.IsDefined(typeof(DataByteType), (DataByteType)client.Data[SharedProperties.TypeByte]))
				{
					int length = BitConverter.ToInt32(new byte[] { client.Data[SharedProperties.LengthByte1], client.Data[SharedProperties.LengthByte2], 0, 0 }, 0);
					int series = BitConverter.ToInt32(new byte[] { client.Data[SharedProperties.SeriesByte1], client.Data[SharedProperties.SeriesByte2], 0, 0 }, 0);
					Guid guid = new Guid(client.Data.SubArray(SharedProperties.GuidStartByte, 16));

					DataBufferModel buffer = Buffers.FirstOrDefault(n => n.DataId == guid);
					if(buffer != null)
					{
						buffer.BufferedData.Add(series, client.Data.SubArray(SharedProperties.HeaderByteSize, SharedProperties.DataLength));
						buffer.LatestSeries = series;
					}
					else
					{
						buffer = new DataBufferModel();
						buffer.BufferedData.Add(series, client.Data.SubArray(SharedProperties.HeaderByteSize, SharedProperties.DataLength));
						buffer.DataId = guid;
						buffer.SeriesLength = length;
						buffer.LatestSeries = series;
						Buffers.Add(buffer);
					}
					BConsole.WriteLine($"Received data with id: {guid.ToString()}");
					
					if(buffer.BufferedData.Count == buffer.SeriesLength)
					{				
						if (HandleIncomingData(ClientServerPipeline.BufferDeserialize(buffer), client, type))
						{
							Buffers.Remove(buffer);
						}
					}
				}
				clientSocket.BeginReceive(client.Data, 0, dataSize, SocketFlags.None, new AsyncCallback(ReceiveData), clientSocket);
			}
			catch (SocketException)
			{
				Socket clientSocket = (Socket)result.AsyncState;
				Client client = GetClientBySocket(clientSocket);
				KickClient(client);
			}
			catch (Exception e)
			{
				BConsole.WriteLine("Error occured: " + e.Message, ConsoleColor.Red);
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
			BConsole.WriteLine("Sending data with id: " + message.DataId.ToString());
			byte[] lengthByteArray = BitConverter.GetBytes(message.SeriesLength);
			foreach (KeyValuePair<int, byte[]> item in message.BufferedData)
			{
				byte[] seriesByteArray = BitConverter.GetBytes(item.Key);
				byte[] sendArray = new byte[] { (byte)type, lengthByteArray[0], lengthByteArray[1], seriesByteArray[0], seriesByteArray[1] };
				sendArray = sendArray.Concat(message.DataId.ToByteArray()).Concat(item.Value).ToArray();
				SendBytesToSocket(s, sendArray);
			}
		}

		/// <summary>
		/// Function that checks for the client you selected and then according to that sends the object.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="message"></param>
		public void SendDataObjectToSelectedClient(DataByteType type, DataBufferModel message)
		{
			if (allClientsSelected)
			{
				SendDataObjectToAll(type, message);
			}
			else
			{
				SendDataObjectToSocket(type, GetSocketByClient(SelectedClient), message);
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

		/// <summary>
		/// Function that can send data and then receive a callback
		/// </summary>
		/// <param name="result"></param>
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
				BConsole.WriteLine("Error occured: " + e.Message.Substring(0, 20));
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
			try
			{
				return clients.FirstOrDefault(x => x.Value.GetClientID() == client.GetClientID()).Key;
			}
			catch
			{
				BConsole.WriteLine($"Socket for {client.GetName()} not found", ConsoleColor.Red);
				return null;
			}
		}

		/// <summary>
		/// Kicks the specified client from the server.
		/// </summary>
		/// <param name="client">The client.</param>
		public void KickClient(Client client)
		{
			Socket s = GetSocketByClient(client);
			if (s != null)
				CloseSocket(s);
			
			if(client != null)
				ClientDisconnected(client);
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
		public void SendDataObjectToAll(DataByteType type, DataBufferModel message)
		{
			foreach(Socket s in clients.Keys)
			{
				try
				{
					SendDataObjectToSocket(type, s, message);
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
