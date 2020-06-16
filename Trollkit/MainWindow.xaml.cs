using System.Windows;
using System.Windows.Input;

namespace Trollkit
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
			if(e.RightButton != MouseButtonState.Pressed)
            this.DragMove();
        }
    }
}
