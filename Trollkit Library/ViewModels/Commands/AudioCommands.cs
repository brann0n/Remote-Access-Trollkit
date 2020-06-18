using System;
using System.Windows.Input;
using Trollkit_Library.Models;
using Trollkit_Library.Modules;
using Trollkit_Library.ServerModules;

namespace Trollkit_Library.ViewModels.Commands
{
	public class AudioCommands
	{
		private readonly Server _server;
		private readonly string handler;

		public AudioCommands(Server server, string handler)
		{
			_server = server;
			this.handler = handler;
		}

		public ICommand Beep { get { return new SendServerCommand(SendBeep); } }
		public ICommand MyNameIsJeff { get { return new SendServerCommand(SendMyNameIsJeff); } }
		public ICommand VolumeUp { get { return new SendServerCommand(SendVolumeUp); } }
		public ICommand VolumeDown { get { return new SendServerCommand(SendVolumeDown); } }
		public ICommand WesselMove { get { return new SendServerCommand(SendWesselMove); } }
		public ICommand Windows { get { return new SendServerCommand(SendWindows); } }
		public ICommand Horn { get { return new SendServerCommand(SendHorn); } }
		public ICommand Macintosh { get { return new SendServerCommand(SendMacintosh); } }
		public ICommand PlayPause { get { return new SendServerCommand(SendPlayPauze); } }
		public ICommand Next { get { return new SendServerCommand(SendNext); } }
		public ICommand Previous { get { return new SendServerCommand(SendPrev); } }
		public ICommand Mute { get { return new SendServerCommand(SendMute); } }
		public ICommand StopSound { get { return new SendServerCommand(SendStop); } }

		/// <summary>
		/// Sends the beep command to the client
		/// </summary>
		public void SendBeep()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "PlayBeep", Handler = handler, Value = "800,800" };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		/// <summary>
		/// Sends the MyNameIsJeff command to the client
		/// </summary>
		private void SendMyNameIsJeff()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "Jeff", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		/// <summary>
		/// Sends the VolumeUp Command to the client
		/// </summary>
		private void SendVolumeUp()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "VolumeUp", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		/// <summary>
		/// Sends the volume down command to the client
		/// </summary>
		private void SendVolumeDown()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "VolumeDown", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		/// <summary>
		/// Sends the WesselMove command to the client
		/// </summary>
		private void SendWesselMove()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "WesselMove", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		/// <summary>
		/// Sends the Windows Notification command to the client
		/// </summary>
		private void SendWindows()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "Windows", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		/// <summary>
		/// Sends the Horn command to the client
		/// </summary>
		private void SendHorn()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "Horn", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		/// <summary>
		/// Sends the vaporwave music command to the client
		/// </summary>
		private void SendMacintosh()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "Macintosh", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}
		
		/// <summary>
		/// Sends the play or pause command to the client
		/// </summary>
		private void SendPlayPauze()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "PlayPause", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		/// <summary>
		/// Sends the next track command to the client
		/// </summary>
		private void SendNext()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "NextTrack", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		/// <summary>
		/// Sends the Mute command to the client
		/// </summary>
		private void SendMute()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "Mute", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		/// <summary>
		/// Sends the Previous track commmand to the client
		/// </summary>
		private void SendPrev()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "PreviousTrack", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		/// <summary>
		/// Sends the music stop command to the client
		/// </summary>
		private void SendStop()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "Stop", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}
	}
}
