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
using Trollkit_Library.ClientModules;

namespace Trollkit.Windows
{
    /// <summary>
    /// Interaction logic for SystemInfo.xaml
    /// </summary>
    public partial class SystemInfo : TrollControl
	{
        public SystemInfo() : base("System")
        {
            InitializeComponent();
        }
    } 
}
