using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Trollkit_Library.Models;
using Trollkit_Library.Modules;
using Trollkit_Library.ViewModels;
using Trollkit_Library.ViewModels.Commands;
using static Trollkit_Library.ServerModules.Server;

namespace Trollkit_Library.ClientModules
{
	public class Client : INotifyPropertyChanged
	{
		private IPEndPoint endPoint;
		private uint id;
		private string receivedData;
		private DateTime connectedAt;
		private string Name;
		public string ClientName { get { return Name; } }

		public byte[] Data;

		/// <summary>
		/// Event that occures when a list object is updated
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		public Dictionary<string, string> storedData { get; set; }

		public string IpAddress { get { return endPoint.Address.ToString(); } }

        public ICommand Kick { get { return new SendServerCommand(KickClient); } }
        public ICommand Select { get { return new SendServerCommand(SelectClient); } }
		public ICommand RemoveVirus { get { return new SendServerCommand(RemoveVirusFromClient); } }

		public string CMDBuffer { get; set; }
		public string ScreenshotString { get; set; }

		public void AddToCMDBuffer(string data)
		{
			CMDBuffer += data + "\r\n";
			NotifyPropertyChanged("CMDBuffer");
		}

		public void SetScreenshot(string base64Image)
		{
			ScreenshotString = base64Image;
			NotifyPropertyChanged("ScreenshotString");
		}

		private void RemoveVirusFromClient()
		{
			TransferCommandObject removeVirusTransferObject = new TransferCommandObject { Command = "DeleteTask", Handler = "Task" };
			var socket = ServerViewModel.Server.GetSocketByClient(this);
			ServerViewModel.Server.SendDataObjectToSocket(DataByteType.Command, socket, ClientServerPipeline.BufferSerialize(removeVirusTransferObject));
		}

		public void KickClient()
        {
            ServerViewModel.Server.KickClient(this);
        }

        public void SelectClient()
        {
            ServerViewModel.Server.SelectedClient = this;
        }

        public Client(uint id, IPEndPoint pAddressEndpoint)
		{
			this.id = id;
			this.connectedAt = DateTime.Now;
			this.endPoint = pAddressEndpoint;
			SetName($"Client #{id}");
			NotifyPropertyChanged("ClientName");
			this.Data = new byte[SharedProperties.DataSize];
			storedData = new Dictionary<string, string>();
			CMDBuffer = "";
		}

		public void SetDataItem(string key, string value)
		{
			storedData[key] = value;
			NotifyPropertyChanged("storedData");
		}

		public string GetDataItem(string key)
		{
			return storedData[key];
		}

		public string GetName()
		{
			return Name;
		}

		public void SetName(string name)
		{
			Name = name;
			NotifyPropertyChanged("ClientName");
		}

		/// <summary>
		/// Gets the client identifier.
		/// </summary>
		/// <returns>Client's identifier.</returns>
		public uint GetClientID()
		{
			return id;
		}

		/// <summary>
		/// Gets the remote address.
		/// </summary>
		/// <returns>Client's remote address.</returns>
		public IPEndPoint GetRemoteAddress()
		{
			return endPoint;
		}

		/// <summary>
		/// Gets the client's last received data.
		/// </summary>
		/// <returns>Client's last received data.</returns>
		public string GetReceivedData()
		{
			return receivedData;
		}
		/// <summary>
		/// Sets the client's last received data.
		/// </summary>
		/// <param name="newReceivedData">The new received data.</param>
		public void SetReceivedData(string newReceivedData)
		{
			this.receivedData = newReceivedData;
		}

		/// <summary>
		/// Appends a string to the client's last
		/// received data.
		/// </summary>
		/// <param name="dataToAppend">The data to append.</param>
		public void AppendReceivedData(string dataToAppend)
		{
			this.receivedData += dataToAppend;
		}

		/// <summary>
		/// Removes the last character from the
		/// client's last received data.
		/// </summary>
		public void RemoveLastCharacterReceived()
		{
			receivedData = receivedData.Substring(0, receivedData.Length - 1);
		}

		/// <summary>
		/// Resets the last received data.
		/// </summary>
		public void ResetReceivedData()
		{
			receivedData = string.Empty;
		}

		public override string ToString()
		{
			string ip = string.Format("{0}:{1}", endPoint.Address.ToString(), endPoint.Port);

			string res = string.Format("Client #{0} (From: {1}, Connection time: {2})", id, ip, connectedAt);

			return res;
		}

		/// <summary>
		/// The important notifier method of changed properties. This function should be called whenever you want to inform other classes that some property has changed.
		/// </summary>
		/// <param name="propertyName">The name of the updated property. Leaving this blank will fill in the name of the calling property.</param>
		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
