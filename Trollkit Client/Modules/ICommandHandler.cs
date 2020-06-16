using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Trollkit_Library;
using Trollkit_Library.Models;
using static Trollkit_Library.ServerModules.Server;

namespace Trollkit_Client.Modules
{
    public abstract class ICommandHandler
    {
        public abstract bool HandleCommand(Socket s, TransferCommandObject obj);

		public void SendCommandObjectToSocket(Socket s, DataBufferModel message)
		{
			SendObjectToSocket(s, message, 0x1B);
		}

		public void SendDataObjectToSocket(Socket s, DataBufferModel message)
		{
			SendObjectToSocket(s, message, 0x1C);
		}

		public void SendResponseObjectToSocket(Socket s, DataBufferModel message)
		{
			SendObjectToSocket(s, message, 0x1A);
		}

		private void SendObjectToSocket(Socket s, DataBufferModel message, byte type)
		{
			BConsole.WriteLine("Sending data with id: " + message.DataId.ToString());

			byte[] lengthByteArray = BitConverter.GetBytes(message.SeriesLength);

			foreach (KeyValuePair<int, byte[]> item in message.BufferedData)
			{
				byte[] seriesByteArray = BitConverter.GetBytes(item.Key);

				byte[] sendArray = new byte[] { type, lengthByteArray[0], lengthByteArray[1], seriesByteArray[0], seriesByteArray[1] };
				sendArray = sendArray.Concat(message.DataId.ToByteArray()).Concat(item.Value).ToArray();
				s.Send(sendArray, 0, sendArray.Length, SocketFlags.None);
			}
			//s.Send(item.Value, 0, item.Value.Length, SocketFlags.None);
		}	
	}
}
