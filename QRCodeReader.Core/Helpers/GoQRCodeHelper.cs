using System;
using QRCodeReader.Core.Models;


namespace QRCodeReader.Core.Helpers
{
    public static class GoQRCodeHelper
    {
        public static GoQRCodeFromFileData ToGoQRCodeFromFileData(this QRCodeFromFile fromFile)
        {
            return new GoQRCodeFromFileData
            {
                File = fromFile.Stream,
                Size = fromFile.Size,
                Name = fromFile.Name
            };
        }
    }
}
