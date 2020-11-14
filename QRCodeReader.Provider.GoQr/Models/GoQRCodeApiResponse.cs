using System.Collections.Generic;

namespace QRCodeReader.Provider.GoQr.Models
{
    public class GoQRCodeApiResponse
    {
        public string Type { get; set; }
        public IEnumerable<GoQRCodeApiSymbol> Symbol { get; set; }
    }
}
