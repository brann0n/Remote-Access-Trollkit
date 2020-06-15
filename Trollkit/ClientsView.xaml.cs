using System;
using System.Collections.Generic;
using System.IO;
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
using Trollkit_Library.ClientModules;

namespace Trollkit
{
    /// <summary>
    /// Interaction logic for ClientsView.xaml
    /// </summary>
    public partial class ClientsView : UserControl
    {
        public ClientsView()
        {
            InitializeComponent();


            //this.ddos();
        }

        public void addClient(Client client)
        {
            Button button = new Button();

            button.Content = client.GetRemoteAddress();
            button.Click += (s, e) => {
                ((MainWindow)Application.Current.MainWindow).server.SelectedClient = client;
            };
            clientsList.Children.Add(button);
        }

        public void clearClients()
        {
            clientsList.Children.Clear();
        }

        public void ddos()
        {
            //je kunt ook een apparaat/router targeten.

            string html = string.Empty;
            string url = @"https://bramgerrits.com";
            HttpWebResponse response;

            //while (true)
            //{

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
                Console.WriteLine("Request status: " + response.StatusCode + " " + DateTime.Now);
            //}

            //Console.WriteLine("Request status: " + response.StatusCode + " Server: " + response.Server);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.clearClients();
            MainWindow main = (MainWindow)Application.Current.MainWindow;
            List<Client> clients = main.server.GetClients();

            foreach (Client client in clients)
            {
                this.addClient(client);
            }
        }

        public void displayClients()
        {
            this.clearClients();
            MainWindow main = (MainWindow)Application.Current.MainWindow;
            List<Client> clients = main.server.GetClients();

            foreach (Client client in clients)
            {
                this.addClient(client);
            }
        }

    }
}
