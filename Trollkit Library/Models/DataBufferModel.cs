using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trollkit_Library.Models
{
	public class DataBufferModel
	{
		public Guid DataId { get; set; }
		public int SeriesLength { get; set; }
		public int LatestSeries { get; set; }


		public Dictionary<int, byte[]> BufferedData { get; set; }

		public DataBufferModel()
		{
			BufferedData = new Dictionary<int, byte[]>();
		}
	}
}
