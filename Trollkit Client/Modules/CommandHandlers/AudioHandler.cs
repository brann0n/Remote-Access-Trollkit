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
		public bool HandleCommand(TransferCommandObject obj)
		{
			if (obj.Command == "PlayBeep")
			{
				string[] values = obj.Value.Split(',');

				if(values.Length == 2)
				{
					Console.Beep(int.Parse(values[0]), int.Parse(values[1]));
				}
				else
				{
					Console.Beep(500, 500);
				}

				return true;
			}

			return false;
		}
	}
}
