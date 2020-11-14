using System.Collections.Generic;

namespace QRCodeReader.Core.Models
{
    public class GoQRCodeProviderConfiguration
    {
        public IEnumerable<string> BaseUrl { get; internal set; }
        public string Version { get; internal set; }
        public string ReadResourceUri { get; internal set; }
    }
}
