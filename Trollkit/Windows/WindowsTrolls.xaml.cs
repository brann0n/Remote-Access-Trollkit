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
using Trollkit_Library.Models;
using Trollkit_Library.Modules;

namespace Trollkit.Windows
{
    /// <summary>
    /// Interaction logic for WindowsTrolls.xaml
    /// </summary>
    public partial class WindowsTrolls : TrollControl
    {
        public WindowsTrolls() :base("Windows")
        {
            InitializeComponent();
        }
    }
}
