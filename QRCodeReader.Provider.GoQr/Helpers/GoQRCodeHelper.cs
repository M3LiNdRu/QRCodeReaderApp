using QRCodeReader.Core.Models;
using QRCodeReader.Provider.GoQr.Models;

namespace QRCodeReader.Provider.GoQr.Helpers
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
