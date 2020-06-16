using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using Trollkit_Library.Models;

namespace Trollkit.Windows
{
    /// <summary>
    /// Interaction logic for VisualTrolls.xaml
    /// </summary>
    public partial class VisualTrolls : TrollControl
    {	
        public VisualTrolls() : base("Visuals")
        {
            InitializeComponent();
        }

		private void BtnDisplayImage_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog open = new OpenFileDialog();
			open.Filter = "Image Files(*.png; *.jpg; *.jpeg; *.gif;)|*.png; *.jpg; *.jpeg; *.gif;";
			open.Multiselect = false;
			open.Title = "Pick an image to send to the client";
			if(open.ShowDialog() == true)
			{
				byte[] bytes = File.ReadAllBytes(open.FileName);

				string base64 = Convert.ToBase64String(bytes);
				TransferCommandObject returnObject = new TransferCommandObject { Command = "ShowImage", Handler = GetHandler(), Value = base64 };

				//App.Server.SendDataObjectToAll(ClientServerPipeline.BufferSerialize(returnObject));
			}		
		}

		private void BtnDisplayText_Click(object sender, RoutedEventArgs e)
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "TextBox", Handler = GetHandler(), Value = tbDisplayText.Text };
			//App.Server.SendDataObjectToAll(ClientServerPipeline.BufferSerialize(returnObject));
        }

		private void BtnTurnOffScreen_Click(object sender, RoutedEventArgs e)
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "BlackScreen", Handler = GetHandler(), Value = "" };
			//App.Server.SendDataObjectToAll(ClientServerPipeline.BufferSerialize(returnObject));
		}

		private void BtnOpenSite_Click(object sender, RoutedEventArgs e)
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "OpenSite", Handler = GetHandler(), Value = tbOpenSite.Text };
			//App.Server.SendDataObjectToAll(ClientServerPipeline.BufferSerialize(returnObject));
		}

		private void BtnPickBackgroundImage_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog open = new OpenFileDialog();
			open.Filter = "Image Files(*.png; *.jpg; *.jpeg;)|*.png; *.jpg; *.jpeg;";
			open.Multiselect = false;
			open.Title = "Pick an image to set as background in the client";
			if (open.ShowDialog() == true)
			{
				byte[] bytes = File.ReadAllBytes(open.FileName);

				string base64 = Convert.ToBase64String(bytes);

				TransferCommandObject returnObject = new TransferCommandObject { Command = "SetBackground", Handler = GetHandler(), Value = base64 };
				//App.Server.SendDataObjectToAll(ClientServerPipeline.BufferSerialize(returnObject));
			}			
		}
	}
}
