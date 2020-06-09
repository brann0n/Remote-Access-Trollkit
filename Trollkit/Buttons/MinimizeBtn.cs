using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Trollkit.Buttons
{
	public class MinimizeBtn : Button
	{
		static MinimizeBtn()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(MinimizeBtn), new
			FrameworkPropertyMetadata(typeof(MinimizeBtn)));
		}

		protected override void OnClick()
		{
			WindowCollection windows = Application.Current.Windows;

			foreach (Window window in windows)
			{
				window.WindowState = WindowState.Minimized;
			}
		}
	}
}
