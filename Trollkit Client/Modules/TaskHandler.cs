using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Trollkit_Library.Models;

namespace Trollkit_Client.Modules
{
    public class TaskHandler : ICommandHandler
    {
        public void HandleCommand(TransferCommandObject obj)
        {
            if(obj.Command == "DeleteTask")
            {
                TaskSchedulerHelper tsk = new TaskSchedulerHelper();
                tsk.DeleteTask();
            }
        }
    }
}
