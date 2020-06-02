using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Trollkit_Library.Models;
using Trollkit_Library.Modules;

namespace Trollkit_Library
{
	public class ClientReceiver
	{
		List<DataBufferModel> buffers;
		private Socket remoteSocket;
		public delegate void DataReceivedHandler(Socket s, TransferCommandObject Object);

		public event DataReceivedHandler OnDataReceived;

		public ClientReceiver()
		{
			buffers = new List<DataBufferModel>();
		}


		public void ConnectAndReceive(string ip)
		{
			remoteSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			remoteSocket.Connect(new IPEndPoint(IPAddress.Parse(ip), 6969));
			BConsole.WriteLine($"Connected to Trollkit host at {ip}:6969");
			while (true)
			{
				//receive data from server.
				byte[] array = new byte[2048];
				try
				{
					remoteSocket.Receive(array);
				}
				catch
				{
					BConsole.WriteLine("Crashed");
					break;
				}
				int length = array[1];
				int series = array[2];
				byte[] guidBytes = Extensions.SubArray(array, 3, 16);
				Guid guid = new Guid(guidBytes);
				if(guid != Guid.Empty)
				{
					DataBufferModel buffer = buffers.FirstOrDefault(n => n.DataId == guid);
					if (buffer != null)
					{
						buffer.BufferedData.Add(series, Extensions.SubArray(array, 19, 2028));
						buffer.LatestSeries = series;
					}
					else
					{
						buffer = new DataBufferModel();
						buffer.BufferedData.Add(series, Extensions.SubArray(array, 19, 2028));
						buffer.LatestSeries = series;
						buffer.DataId = guid;
						buffer.SeriesLength = length;
						buffers.Add(buffer);
					}

					if(buffer.BufferedData.Count == buffer.SeriesLength)
					{
						OnDataReceived?.Invoke(remoteSocket, ClientServerPipeline.BufferDeserialize(buffer));
					}

				}
			}
		}


		public void SendCommandObjectToSocket(DataBufferModel message)
		{
			BConsole.WriteLine("Sending data with id: " + message.DataId.ToString());

			foreach (KeyValuePair<int, byte[]> item in message.BufferedData)
			{
				byte[] sendArray = new byte[] { (byte)0x1b, (byte)message.SeriesLength, (byte)item.Key };
				sendArray = sendArray.Concat(message.DataId.ToByteArray()).Concat(item.Value).ToArray();
				remoteSocket.Send(item.Value, 0, item.Value.Length, SocketFlags.None);
			}
		}
	}
}
