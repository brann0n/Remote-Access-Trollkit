using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trollkit_Library;

namespace Trollkit_Client
{
	class Program
	{
		private ClientDiscovery discover;
		static void Main(string[] args)
		{
			var program = new Program();
			var addresses = program.discover.GetIpAddresses();
			string ip = program.discover.GetRemoteServerIp(addresses);

			Console.WriteLine(ip);
			Console.Read();
		}

		public Program()
		{
			discover = new ClientDiscovery("gang?", "Dopple gang");
		}


	}
}
