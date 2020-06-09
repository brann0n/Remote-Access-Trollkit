using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trollkit_Library.Models;

namespace Trollkit_Client.Modules.CommandHandlers
{
    class SystemInfoHandler : ICommandHandler
    {
        public void HandleCommand(TransferCommandObject obj)
        {
            Console.WriteLine(getSysInfo());
        }

        private string getSysInfo()
        {
            string returnString = "Available Drives: ";
            var drives = DriveInfo.GetDrives();
            foreach (DriveInfo info in drives)
            {
                if (info.IsReady)
                {
                    returnString += $"({info.Name.Replace("\\", "")}) ";
                }
            }
            return returnString;
        }
    }
}
