using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trollkit_Library.Models
{
	public class ScreenTypeModel
	{
		public int ScreenId { get; set; }
		public string ScreenName { get; set; }

		public override string ToString()
		{
			return $"{ScreenId} | {ScreenName}";
		}
	}
}
