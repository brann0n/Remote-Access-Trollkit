using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Trollkit_Library.Modules
{
	public static class Extensions
	{
		public static bool isType(this Type type1, Type type2)
		{
			return type1 == type2;
		}

		public static bool ImplementsInterface(this Type type, Type ifaceType)
		{
			Type[] intf = type.GetInterfaces();
			for (int i = 0; i < intf.Length; i++)
			{
				if (intf[i] == ifaceType)
				{
					return true;
				}
			}
			return false;
		}

		public static Tarray[] SubArray<Tarray>(this Tarray[] data, int index, int length)
		{
			Tarray[] result = new Tarray[length];
			if (length > data.Length - index)
			{
				Array.Copy(data, index, result, 0, data.Length - index);
			}
			else
			{
				Array.Copy(data, index, result, 0, length);
			}

			return result;
		}

		public static byte[] ToByteArray(this Image image, ImageFormat format)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				image.Save(ms, format);
				return ms.ToArray();
			}
		}

		public static bool IsConnected(this Socket socket)
		{
			try
			{
				return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
			}
			catch (SocketException) { return false; }
		}
	}
}
