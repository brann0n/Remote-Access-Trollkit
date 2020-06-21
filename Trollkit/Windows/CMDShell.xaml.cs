using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Trollkit.Windows
{
    /// <summary>
    /// Interaction logic for CMDShell.xaml
    /// </summary>
    public partial class CMDShell : UserControl
    {
        public CMDShell()
        {
            InitializeComponent();
        }

		/// <summary>
		/// This has to be in the codebehind, there is no other option to move the caret on text change
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TextboxCMD_TextChanged(object sender, TextChangedEventArgs e)
		{
			var caretLocation = textboxCMD.GetRectFromCharacterIndex(textboxCMD.CaretIndex).Location;
			if (!double.IsInfinity(caretLocation.X))
			{
				Canvas.SetLeft(Caret, caretLocation.X);
			}

			if (!double.IsInfinity(caretLocation.Y))
			{
				Canvas.SetTop(Caret, caretLocation.Y);
			}
		}
	}
}
