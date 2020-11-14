using System.Collections.Generic;

namespace QRCodeReader.Core.Models
{
    public class GoQRCodeApiResponse
    {
        public string Type { get; set; }
        public IEnumerable<GoQRCodeApiSymbol> Symbol { get; set; }
    }
}
