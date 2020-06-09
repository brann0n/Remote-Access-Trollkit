using System.Windows;
using System.Windows.Controls;

namespace Trollkit.Buttons
{
	public class HeadMenuBtn : Button
	{
		static HeadMenuBtn()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(HeadMenuBtn), new
			FrameworkPropertyMetadata(typeof(HeadMenuBtn)));
		}

		protected override void OnClick()
		{
			ContentControl trollView = (ContentControl)this.FindName("trollView");
			trollView.Content = this.Resources["View"];
		}
	}
}
