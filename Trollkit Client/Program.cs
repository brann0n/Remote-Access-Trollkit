using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trollkit_Library;
using Trollkit_Library.Models;

namespace Trollkit_Client
{
	class Program
	{
		private ClientDiscovery discover;
		private ClientReceiver receiver;

		static void Main(string[] args)
		{
			var program = new Program();
			var addresses = program.discover.GetIpAddresses();
			string ip = program.discover.GetRemoteServerIp(addresses);
			program.receiver.ConnectAndReceive(ip);
			Console.Read();
		}

		public Program()
		{
			discover = new ClientDiscovery("gang?", "Dopple gang");
			receiver = new ClientReceiver();
			receiver.OnDataReceived += Receiver_OnDataReceived;
		}

		private void Receiver_OnDataReceived(TransferCommandObject Object)
		{
			throw new NotImplementedException();
		}
	}
}
