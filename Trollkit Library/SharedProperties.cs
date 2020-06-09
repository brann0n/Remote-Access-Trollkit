using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trollkit_Library
{
	public static class SharedProperties
	{
		public const int BroadcastPort = 9696;
		public const int MainPort = 6969;
		public const int DataSize = 2048;

		public const int TypeByte = 0;
		public const int LengthByte1 = 1;
		public const int LengthByte2 = 2;
		public const int SeriesByte1 = 3;
		public const int SeriesByte2 = 4;
		public const int GuidStartByte = 5; // 16 bytes long
		public const int HeaderByteSize = 21; //top results into 21

		public static int DataLength { get { return DataSize - HeaderByteSize; } }
	}
}
