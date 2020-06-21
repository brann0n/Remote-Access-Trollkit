using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trollkit_Library.Models
{
	public class ScreenshotModel
	{
		public string ScreenshotData { get; set; }
		public DateTime Timestamp { get; set; }

		public string ConvertToJson()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
