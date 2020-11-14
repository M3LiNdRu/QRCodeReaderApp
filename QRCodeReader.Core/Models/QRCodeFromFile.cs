using System.IO;

namespace QRCodeReader.Core.Models
{
    public class QRCodeFromFile
    {
        public string Name { get; set; }
        public Stream Stream { get; set; }
        public int Size { get; set; }
    }
}