using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Trollkit_Library.Models;

namespace Trollkit_Library
{
	class Server
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
		public delegate void ClientMessageReceivedHandler(Client c, TransferCommandObject model, DataEventType type);

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
		/// Returns a list of Connected clients
		/// </summary>
		/// <returns></returns>
		public List<Client> GetClients()
		{
			return clients.Values.ToList();
		}
	}
}
