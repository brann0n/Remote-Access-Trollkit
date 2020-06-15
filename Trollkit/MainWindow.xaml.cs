using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Trollkit.Windows;
using Trollkit_Library;
using Trollkit_Library.ClientModules;
using Trollkit_Library.Models;
using Trollkit_Library.Modules;
using Trollkit_Library.ServerModules;

namespace Trollkit
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public Server server;

		public MainWindow()
		{
			InitializeComponent();
			server = new Server(IPAddress.Any);
			server.ClientConnected += Server_ClientConnected;
			server.ClientDisconnected += Server_ClientDisconnected;
			server.MessageReceived += Server_MessageReceived;
			server.Start();

			Task.Run(() => new ServerDiscovery("gang?", "Dopple gang").Discover());

            ((ContentControl)this.FindName("trollView")).Content = new ClientsView();
        }

		private void Server_MessageReceived(Client c, TransferCommandObject model, Server.DataByteType type)
		{
			BConsole.WriteLine($"Client {c.GetName()} sent a message", ConsoleColor.DarkGray);

			if(model.Command == "Debug")
			{
				TransferCommandObject returnObject = new TransferCommandObject { Command = "PlayBeep", Handler = "Audio", Value = "200,300" };

				server.SendDataObjectToSocket(Server.DataByteType.Command, server.GetSocketByClient(c), ClientServerPipeline.BufferSerialize(returnObject));
			}
		}

		private void Server_ClientDisconnected(Client c)
		{
			BConsole.WriteLine($"Client {c.GetName()} has disconnected!", ConsoleColor.Yellow);
		}

		private void Server_ClientConnected(Client c)
		{
			BConsole.WriteLine($"Client {c.GetName()} has connected!", ConsoleColor.Yellow);
			if(server.SelectedClient != null)
			{
				server.SelectedClient = c;
			}
		}

        private void Drag(object sender, MouseButtonEventArgs e)
        {
			if(e.RightButton != MouseButtonState.Pressed)
            this.DragMove();
        }
    }
}
