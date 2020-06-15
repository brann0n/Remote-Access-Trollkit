using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Trollkit.Windows
{
	public class TrollControl : UserControl
	{
		private string Handler { get; set; }

		public TrollControl(string handler) : base()
		{
			this.Handler = handler;
		}

		public string GetHandler()
		{
			return this.Handler;
		}
	}
}
