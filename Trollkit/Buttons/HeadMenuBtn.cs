using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Trollkit.Buttons
{
	public class HeadMenuBtn : Button
	{

		private static HeadMenuBtn focussedBtn;
		private bool btnDown;

		static HeadMenuBtn()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(HeadMenuBtn), new
			FrameworkPropertyMetadata(typeof(HeadMenuBtn)));
		}

		protected override void OnMouseLeave(MouseEventArgs e)
		{			
			base.OnMouseLeave(e);

			if (btnDown)
			{
				if(focussedBtn != null)
				{
					focussedBtn.Focus();
				}
				else
				{

				}
			}
		}

		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			btnDown = true;
		}

		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonUp(e);
			btnDown = false;
		}

		protected override void OnClick()
		{
			ContentControl trollView = (ContentControl)this.FindName("trollView");
			trollView.Content = this.Resources["View"];
			focussedBtn = this;
			//Focus();
		}
	}
}
