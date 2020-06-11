using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Trollkit_Library.ClientModules
{
	public class ClientDiscovery
	{
		private string BroadcastMessage { get; set; }
		private string ExpectedReturnMessage { get; set; }
		public ClientDiscovery(string broadcastMessage, string expectedReturnMessage)
		{
			BroadcastMessage = broadcastMessage;
			ExpectedReturnMessage = expectedReturnMessage;
		}

		public UnicastIPAddressInformationCollection GetIpAddresses()
		{
			UnicastIPAddressInformationCollection BroadcastAddresses = null;

			NetworkInterface[] Interfaces = NetworkInterface.GetAllNetworkInterfaces();

			foreach (NetworkInterface Interface in Interfaces)
			{
				IPInterfaceProperties props = Interface.GetIPProperties();
				if (props.GatewayAddresses.Count > 0 && Interface.OperationalStatus == OperationalStatus.Up)
				{
					BroadcastAddresses = props.UnicastAddresses; //0.0.0.255
				}

			}

			return BroadcastAddresses;
		}

		public string GetRemoteServerIp(UnicastIPAddressInformationCollection addresses)
		{
			string ip = "";
			string response = "";
			while (true)
			{
				//check if the correct response has been received from the remote server, if true, abort the while loop.
				if (response == ExpectedReturnMessage) break;

				//setup the new udp client
				UdpClient Client = new UdpClient();
				Client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
				Client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

				//the data and server endpoint for later use
				byte[] RequestData = Encoding.ASCII.GetBytes(BroadcastMessage);
				IPEndPoint ServerEp = new IPEndPoint(IPAddress.Any, 0);

				//loop through each unicast address and send a request to them
				foreach (UnicastIPAddressInformation info in addresses)
				{
					BConsole.WriteLine($"Found Unicast address: {info.Address}");
					if (info.Address.AddressFamily == AddressFamily.InterNetwork)
					{
						IPEndPoint local = new IPEndPoint(info.Address, 0);
						Client.Client.Bind(local);
						Client.Send(RequestData, RequestData.Length, new IPEndPoint(IPAddress.Broadcast, SharedProperties.BroadcastPort));
						BConsole.WriteLine($"Send data to {IPAddress.Broadcast.ToString()} address from {info.Address.ToString()}.", ConsoleColor.Magenta);
					}
				}

				//set the response timout to 5 seconds
				Client.Client.ReceiveTimeout = 5000;
				byte[] ServerResponseData;
				try
				{
					//attempt to receive data from any ip as set by the Server Endpoint.
					//throws an exception after 5 seconds if no response is received.
					ServerResponseData = Client.Receive(ref ServerEp);
					response = Encoding.ASCII.GetString(ServerResponseData);
					BConsole.WriteLine($"Received {response} from {ServerEp.Address.ToString()}");
					ip = ServerEp.Address.ToString();
				}
				catch (SocketException e)
				{
					if (e.SocketErrorCode == SocketError.TimedOut)
					{
						BConsole.WriteLine("Awaiting broadcast response timed out...");
					}
					else if (e.SocketErrorCode == SocketError.AddressAlreadyInUse)
					{
						//this error happens when the local port is in use, this either means the server is using it, or another program.
						//attempt to connect to localhost anyway.
						BConsole.WriteLine("Address was already in use, attempting to connect to localhost");
						response = ExpectedReturnMessage;
						ip = "127.0.0.1";
					}
					else
					{
						//if its something else the error should still be thrown.
						throw e;
					}
				}

				Client.Close();
			}

			return ip;
		}
	}
}
