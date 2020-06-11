using System;
using System.Windows;
using System.Windows.Controls;

namespace Trollkit.Buttons
{
	public class CloseBtn : Button
	{
		static CloseBtn()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(CloseBtn), new
			FrameworkPropertyMetadata(typeof(CloseBtn)));
		}

		protected override void OnClick()
		{
			Environment.Exit(0);
		}
	}
}
