using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Label label = new Label();
            label.Content = client.GetRemoteAddress();
            clientsList.Children.Add(label);
        }
    }
}
