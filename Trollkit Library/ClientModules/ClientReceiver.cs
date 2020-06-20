using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Trollkit_Library.Models;
using Trollkit_Library.Modules;

namespace Trollkit_Library.ClientModules
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

		/// <summary>
		/// Connects to the Remote host and receives data, returns if connection is lost
		/// </summary>
		/// <param name="ip"></param>
		/// <returns>Returns if the client should attempt a reconnect</returns>
		public bool ConnectAndReceive(string ip)
		{
			remoteSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			remoteSocket.Connect(new IPEndPoint(IPAddress.Parse(ip), SharedProperties.MainPort));			
			BConsole.WriteLine($"Connected to Trollkit host at {ip}:{SharedProperties.MainPort}", ConsoleColor.DarkGreen);
			while (remoteSocket.IsConnected())
			{
				//receive data from server.
				byte[] array = new byte[SharedProperties.DataSize];
				try
				{
					remoteSocket.Receive(array);
				}
				catch (SocketException e)
				{
					if(e.SocketErrorCode == SocketError.ConnectionReset)
					{
						BConsole.WriteLine("Host connection closed unexpectedly...", ConsoleColor.Red);
						return true;
					}
					else
					{
						BConsole.WriteLine("Application crashed, closing now");
						return false;
					}
				}
				catch (Exception e)
				{
					BConsole.WriteLine("Application crashed, closing now");
					return false;
				}
				int length = BitConverter.ToInt32(new byte[] { array[SharedProperties.LengthByte1], array[SharedProperties.LengthByte2], 0, 0 }, 0);
				int series = BitConverter.ToInt32(new byte[] { array[SharedProperties.SeriesByte1], array[SharedProperties.SeriesByte2], 0, 0 }, 0);
				byte[] guidBytes = Extensions.SubArray(array, SharedProperties.GuidStartByte, 16);
				Guid guid = new Guid(guidBytes);
				if(guid != Guid.Empty)
				{
					DataBufferModel buffer = buffers.FirstOrDefault(n => n.DataId == guid);
					if (buffer != null)
					{
						buffer.BufferedData.Add(series, Extensions.SubArray(array, SharedProperties.HeaderByteSize, SharedProperties.DataLength));
						buffer.LatestSeries = series;
					}
					else
					{
						buffer = new DataBufferModel();
						buffer.BufferedData.Add(series, Extensions.SubArray(array, SharedProperties.HeaderByteSize, SharedProperties.DataLength));
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

			return false; //if code reaches here, the client was gracefully kicked
		}
	}
}
