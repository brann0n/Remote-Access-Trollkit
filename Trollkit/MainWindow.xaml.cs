using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Trollkit_Library;

namespace Trollkit
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Server server;
		public MainWindow()
		{
			InitializeComponent();
			server = new Server(IPAddress.Any);
			server.ClientConnected += Server_ClientConnected;
			server.ClientDisconnected += Server_ClientDisconnected;
			server.ConnectionBlocked += Server_ConnectionBlocked;
			server.MessageReceived += Server_MessageReceived;
			server.Start();

			Task.Run(() => new ServerDiscovery("gang?", "Dopple gang").Discover());
		}

		private void Server_MessageReceived(Client c, Trollkit_Library.Models.TransferCommandObject model, Server.DataByteType type)
		{
			throw new NotImplementedException();
		}

		private void Server_ConnectionBlocked(IPEndPoint endPoint)
		{
			throw new NotImplementedException();
		}

		private void Server_ClientDisconnected(Client c)
		{
			throw new NotImplementedException();
		}

		private void Server_ClientConnected(Client c)
		{
			throw new NotImplementedException();
		}

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void HeadMenuBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
