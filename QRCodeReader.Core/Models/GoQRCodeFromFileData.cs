using System;
using System.IO;

namespace QRCodeReader.Core.Models
{
    public class GoQRCodeFromFileData
    {
        public Stream File { get; set; }
        public long Size { get; set; }
        public string Name { get; internal set; }
    }
}
