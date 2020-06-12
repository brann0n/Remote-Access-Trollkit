using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;
using Trollkit_Library.Models;
using Trollkit_Library.Modules;

namespace Trollkit_Client.Modules.CommandHandlers
{
	public class AudioHandler : ICommandHandler
	{
		public bool HandleCommand(TransferCommandObject obj)
		{
			Keyboard keyboard = new Keyboard();
			switch (obj.Command)
			{
				case "PlayBeep":
					PlayBeep(obj.Value);
					return true;
				case "Jeff":
					PlayJeff();
					return true;
				case "WesselMove":
					PlayWesselMove();
					return true;
				case "VolumeUp":
					keyboard.Send(Keyboard.ScanCodeShort.VOLUME_UP);
					return true;
				case "VolumeDown":
					keyboard.Send(Keyboard.ScanCodeShort.VOLUME_DOWN);
					return true;
				case "Mute":
					keyboard.Send(Keyboard.ScanCodeShort.VOLUME_MUTE);
					return true;
				case "Play_Pause":
					keyboard.Send(Keyboard.ScanCodeShort.MEDIA_PLAY_PAUSE);
					return true;
				case "NextTrack":
					keyboard.Send(Keyboard.ScanCodeShort.MEDIA_NEXT_TRACK);
					return true;
				case "PreviousTrack":
					keyboard.Send(Keyboard.ScanCodeShort.MEDIA_PREV_TRACK);
					return true;
			}
			return false;
		}

		private void PlayBeep(string playBeep)
		{
			string[] values = playBeep.Split(',');

			if (values.Length == 2)
			{
				Console.Beep(int.Parse(values[0]), int.Parse(values[1]));
			}
			else
			{
				Console.Beep(500, 500);
			}
		}

		private void PlayJeff()
		{
			SoundPlayer soundplayer = new SoundPlayer(Properties.Resources.MyNameIsJeff);
			soundplayer.Play();
		}

		private void PlayWesselMove()
		{
			SoundPlayer soundplayer = new SoundPlayer(Properties.Resources.EchtEenWesselSample);
			soundplayer.Play();
		}

	}
}
