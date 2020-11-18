using System;
namespace QRCodeReader.Provider.GoQr.Models
{
    public class GoQRApiResult
    {
        public bool Success { get; set; }
        public GoQRCodeApiResponse Data { get; set; }
        public string Message { get; set; }
    }
}
