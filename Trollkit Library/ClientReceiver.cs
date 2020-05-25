using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Trollkit_Library
{
	public class ClientReceiver
	{

		public void ConnectAndReceive(string ip)
		{
			Socket remoteSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			remoteSocket.Connect(new IPEndPoint(IPAddress.Parse(ip), 6969));
			Console.WriteLine($"Connected to Trollkit host at {ip}:6969");
			while (true)
			{
				//receive data from server.
				DataTransferObject tObj = new DataTransferObject();
			}
		}
	}
}
