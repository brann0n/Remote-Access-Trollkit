using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Trollkit_Library.Models;

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
					break;
				case "ShowImage":
					break;
				case "OpenSite":
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
	}
}
