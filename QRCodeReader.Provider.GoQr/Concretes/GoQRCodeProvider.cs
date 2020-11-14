using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using QRCodeReader.Core.Interfaces;
using QRCodeReader.Core.Models;
using QRCodeReader.Provider.GoQr.Helpers;
using QRCodeReader.Provider.GoQr.Interfaces;
using QRCodeReader.Provider.GoQr.Models;

namespace QRCodeReader.Provider.GoQr.Concrete
{
    public class GoQRCodeProvider : IQRCodeProvider
    {
        private const string Slash = "/";

        private readonly GoQRCodeProviderConfiguration _configuration;
        private readonly IGoQRCodeClientHelper _restClientHelper;

        public GoQRCodeProvider(IOptionsMonitor<GoQRCodeProviderConfiguration> options, IGoQRCodeClientHelper httpClient)
        {
            _configuration = options.CurrentValue;
            _restClientHelper = httpClient;
        }

        async Task<QRCodeData> IQRCodeProvider.Read(QRCodeFromFile data)
        {
            var result = new QRCodeData();
            var url = BuildReadResourceUri();
            var response = await _restClientHelper.PostImageAsync(url, data.ToGoQRCodeFromFileData());
            result.Data = response.Type;

            return result;
        }

        private string BuildReadResourceUri()
        {
            return string.Concat(_configuration.BaseUrl,
                Slash, _configuration.Version, Slash, _configuration.ReadResourceUri );
        }

        
    }
}
