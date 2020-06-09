using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trollkit_Library.Models;

namespace Trollkit_Client.Modules.CommandHandlers
{
	public class VisualsHandler : ICommandHandler
	{
		public void HandleCommand(TransferCommandObject obj)
		{
			switch (obj.Command)
			{
				case "BlackScreen":
				case "TextBox":
				case "ShowImage":
				case "OpenSite":
				case "SetBackground":
					break;
			}
		}
	}
}
