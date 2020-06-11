using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Trollkit_Library.ClientModules;
using Trollkit_Library.Models;

namespace Trollkit
{
    /// <summary>
    /// Interaction logic for ClientList.xaml
    /// </summary>
    public partial class ClientList : Window
    {
        public ClientList()
        {
            InitializeComponent();
        }

        public void addClient(Client client)
        {
            Button button = new Button();

            button.Content = client.GetRemoteAddress();
            button.Click += (s, e) => {
                ((MainWindow)Application.Current.MainWindow).server.selectClient(client);
            };
            clientsList.Children.Add(button);
        }

        public void ddos()
        {
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
    }
}
