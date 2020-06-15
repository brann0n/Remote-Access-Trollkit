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
		[DllImport("user32.dll")]
		static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, IntPtr dwExtraInfo);

		private const int NEXT_TRACK = 0xB0;
		private const int PLAY_PAUSE = 0xB3;
		private const int PREV_TRACK = 0xB1;
		private const int VOLUME_MUTE = 0xAD;
		private const int VOLUME_UP = 0xAF;
		private const int VOLUME_DOWN = 0xAE;
		public bool HandleCommand(TransferCommandObject obj)
		{
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
					keybd_event(VOLUME_UP, 0, 0, IntPtr.Zero);
					return true;
				case "VolumeDown":
					keybd_event(VOLUME_DOWN, 0, 0, IntPtr.Zero);
					return true;
				case "Mute":
					keybd_event(VOLUME_MUTE, 0, 0, IntPtr.Zero);
					return true;
				case "PlayPause":
					keybd_event(PLAY_PAUSE, 0, 1, IntPtr.Zero);
					return true;
				case "NextTrack":
					keybd_event(NEXT_TRACK, 0, 1, IntPtr.Zero);
					return true;
				case "PreviousTrack":
					keybd_event(PREV_TRACK, 0, 1, IntPtr.Zero);
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
