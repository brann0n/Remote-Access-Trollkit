using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Trollkit_Library
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
				server.Client.Bind(new IPEndPoint(IPAddress.Any, 9696));
				IPEndPoint clientEp = new IPEndPoint(IPAddress.Any, 9696);
				server.Client.EnableBroadcast = true;
				byte[] clientRequestData = server.Receive(ref clientEp);
				string clientRequest = Encoding.ASCII.GetString(clientRequestData);
				if(clientRequest == BroadcastMessage)
				{
					Console.WriteLine($"Received {clientRequest} from {clientEp.Address}, sending response: {responseData}");

					server.Send(responseData, responseData.Length, clientEp);
					server.Close();
				}
				else
				{
					Console.WriteLine($"Received {clientRequest} from {clientEp.Address}, ignoring!");
				}
			}
		}


	}
}
