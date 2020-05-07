using System;
using System.Collections.Generic;
using System.Linq;
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
	}
}
