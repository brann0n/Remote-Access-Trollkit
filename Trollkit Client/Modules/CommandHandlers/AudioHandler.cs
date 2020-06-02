using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trollkit_Library.Models;

namespace Trollkit_Client.Modules.CommandHandlers
{
	public class AudioHandler : ICommandHandler
	{
		public void HandleCommand(TransferCommandObject obj)
		{
			if (obj.Command == "PlayBeep")
			{
				Console.Beep(100, 100);
			}
		}
	}
}
