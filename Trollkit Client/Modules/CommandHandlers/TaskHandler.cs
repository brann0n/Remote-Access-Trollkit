using System;
using System.Collections.Generic;
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
        public bool HandleCommand(Socket s, TransferCommandObject obj)
        {
            if(obj.Command == "DeleteTask")
            {
                TaskSchedulerHelper tsk = new TaskSchedulerHelper();
                tsk.DeleteTask();

				return true;
            }

			return false;
		}
    }
}
