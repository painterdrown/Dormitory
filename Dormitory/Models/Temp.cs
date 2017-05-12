using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Dormitory.Models
{
    class Temp
    {
        public static async Task<byte[]> GetBytesFromStream(IRandomAccessStream stream)
        {
            DataReader read = new DataReader(stream.GetInputStreamAt(0));

            await read.LoadAsync((uint)stream.Size);

            byte[] temp = new byte[stream.Size];

            read.ReadBytes(temp);

            return temp;
        }
    }
}
