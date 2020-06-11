using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trollkit_Library.Models;

namespace Trollkit_Library.Modules
{
	public static class ClientServerPipeline
	{
		public static DataBufferModel BufferSerialize(TransferCommandObject ObjectToSerialize)
		{
			string data = JsonConvert.SerializeObject(ObjectToSerialize);
			byte[] byteArray = Encoding.Default.GetBytes(data);
			DataBufferModel buffer = new DataBufferModel();
			buffer.DataId = Guid.NewGuid();
			int count = 0;
			int bytesLeft = byteArray.Length;
			int index = 0;
			int increment = SharedProperties.DataLength;

			while(bytesLeft > 0)
			{
				count++;
				byte[] subArray = byteArray.SubArray(index, increment);

				bytesLeft -= increment;
				index += increment;

				buffer.BufferedData.Add(count, subArray);
			}

			buffer.SeriesLength = count;

			return buffer;
		}

		public static TransferCommandObject BufferDeserialize(DataBufferModel bufferModel)
		{
			if(bufferModel.BufferedData.Count == bufferModel.SeriesLength)
			{
				byte[] fullBuffer = bufferModel.BufferedData[1]; //index starts at 1 NOT at 0
				if (bufferModel.SeriesLength > 1)
				{				
					for(int i = 2; i <= bufferModel.SeriesLength; i++)
					{
						fullBuffer = fullBuffer.Concat(bufferModel.BufferedData[i]).ToArray();
					}				
				}
				string data = Encoding.Default.GetString(fullBuffer.Where(n => n != 0).ToArray()).Replace("\0", "");
				return JsonConvert.DeserializeObject<TransferCommandObject>(data);
			}

			return default;
		}
	}
}
