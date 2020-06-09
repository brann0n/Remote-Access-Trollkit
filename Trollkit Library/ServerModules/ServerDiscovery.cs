using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Trollkit_Library.ServerModules
{
	public class ServerDiscovery
	{
		private string BroadcastMessage { get; set; }
		private string ExpectedReturnMessage { get; set; }

		public ServerDiscovery(string broadcastMessage, string expectedReturnMessage)
		{
			BroadcastMessage = broadcastMessage;
			ExpectedReturnMessage = expectedReturnMessage;
		}

		public void Discover()
		{
			byte[] responseData = Encoding.ASCII.GetBytes(ExpectedReturnMessage);
			while (true)
			{
				UdpClient server = new UdpClient();
				server.EnableBroadcast = true;
				server.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
				server.Client.Bind(new IPEndPoint(IPAddress.Any, SharedProperties.BroadcastPort));
				IPEndPoint clientEp = new IPEndPoint(IPAddress.Any, SharedProperties.BroadcastPort);
				server.Client.EnableBroadcast = true;
				byte[] clientRequestData = server.Receive(ref clientEp);
				string clientRequest = Encoding.ASCII.GetString(clientRequestData);
				if(clientRequest == BroadcastMessage)
				{
					BConsole.WriteLine($"Received '{clientRequest}' from {clientEp.Address}, sending response: '{ExpectedReturnMessage}'", ConsoleColor.Magenta);

					server.Send(responseData, responseData.Length, clientEp);
					server.Close();
				}
				else
				{
					BConsole.WriteLine($"Received '{clientRequest}' from {clientEp.Address}, ignoring!", ConsoleColor.Red);
				}
			}
		}


	}
}
