using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bittorrent
{
    internal class FileReader
    {
        public byte[] Read(string filePath)
        {
            if(filePath == null) throw new ArgumentNullException(nameof(filePath));

            if (!filePath.EndsWith(".torrent")) throw new ArgumentException("Invalid file extension");

            return File.ReadAllBytes(filePath);
        }
    }
}
