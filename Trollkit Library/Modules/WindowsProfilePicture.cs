﻿using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;

namespace Trollkit_Library.Modules
{
	public class WindowsProfilePicture
	{

		public static string Get448ImageString()
		{
			try
			{
				RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\AccountPicture", true);

				if (key != null)
				{
					string UserName = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
					UserName = Directory.GetParent(UserName).Name;

					string imageId = key.GetValue(@"SourceId").ToString();
					string pfLocation = $@"C:\Users\{UserName}\AppData\Roaming\Microsoft\Windows\AccountPictures\" + imageId + ".accountpicture-ms";

					return Convert.ToBase64String(GetImage448Bytes(pfLocation));
				}
				else
				{
					string defaultImage = @"C:\ProgramData\Microsoft\User Account Pictures\user.bmp";
					if (File.Exists(defaultImage))
					{
						byte[] bytes = File.ReadAllBytes(defaultImage);
						return Convert.ToBase64String(bytes);
					}
					else
					{
						return null;
					}
				}
			}
			catch (Exception e)
			{
				BConsole.WriteLine("ProfilePicture Error: " + e.Message, ConsoleColor.Red);
				return "";
			}
		}

		public static void SaveImagesAsBitmap(string filePath)
		{
			string filename = Path.GetFileNameWithoutExtension(filePath);
			Bitmap image96 = GetBitmapImage(GetImage96Bytes(filePath));
			image96.Save(filename + "-96.bmp");
			Bitmap image448 = GetBitmapImage(GetImage448Bytes(filePath));
			image448.Save(filename + "-448.bmp");
		}

		private static byte[] GetImage96Bytes(string path)
		{
			FileStream fs = new FileStream(path, FileMode.Open);
			long position = Seek(fs, "JFIF", 0);
			byte[] b = new byte[Convert.ToInt32(fs.Length)];
			fs.Seek(position - 6, SeekOrigin.Begin);
			fs.Read(b, 0, b.Length);
			fs.Close();
			fs.Dispose();
			return b;
		}

		private static byte[] GetImage448Bytes(string path)
		{
			FileStream fs = new FileStream(path, FileMode.Open);
			long position = Seek(fs, "JFIF", 100);
			byte[] b = new byte[Convert.ToInt32(fs.Length)];
			fs.Seek(position - 6, SeekOrigin.Begin);
			fs.Read(b, 0, b.Length);
			fs.Close();
			fs.Dispose();
			return b;
		}

		private static Bitmap GetBitmapImage(byte[] imageBytes)
		{
			var ms = new MemoryStream(imageBytes);
			var bitmapImage = new Bitmap(ms);
			return bitmapImage;
		}

		private static long Seek(FileStream fs, string searchString, int startIndex)
		{
			char[] search = searchString.ToCharArray();
			long result = -1, position = 0, stored = startIndex,
			begin = fs.Position;
			int c;
			while ((c = fs.ReadByte()) != -1)
			{
				if ((char)c == search[position])
				{
					if (stored == -1 && position > 0 && (char)c == search[0])
					{
						stored = fs.Position;
					}
					if (position + 1 == search.Length)
					{
						result = fs.Position - search.Length;
						fs.Position = result;
						break;
					}
					position++;
				}
				else if (stored > -1)
				{
					fs.Position = stored + 1;
					position = 1;
					stored = -1;
				}
				else
				{
					position = 0;
				}
			}

			if (result == -1)
			{
				fs.Position = begin;
			}
			return result;
		}
	}
}
