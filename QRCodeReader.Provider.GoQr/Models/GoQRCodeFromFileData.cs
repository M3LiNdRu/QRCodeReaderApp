using System.IO;

namespace QRCodeReader.Provider.GoQr.Models
{
    public class GoQRCodeFromFileData
    {
        public Stream File { get; set; }
        public long Size { get; set; }
        public string Name { get; internal set; }
    }
}
