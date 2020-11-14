using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using QRCodeReader.Core.Interfaces;
using QRCodeReader.Core.Models;

namespace QRCodeReader.Core.Concrete
{
    public class GoQRProvider : IQRCodeProvider
    {
        private readonly GoQRCodeProviderConfiguration _configuration;
        private readonly IGoQRCodeClientHelper _restClientHelper;

        public GoQRProvider(IOptionsMonitor<GoQRCodeProviderConfiguration> options, IGoQRCodeClientHelper httpClient)
        {
            _configuration = options.CurrentValue;
            _restClientHelper = httpClient;
        }

        async Task<QRCodeData> IQRCodeProvider.Read(QRCodeFromFile data)
        {
            var result = new QRCodeData();
            var url = BuildReadResourceUri();
            var response = await _restClientHelper.PostAsync(url, data);


            return result;
        }

        private string BuildReadResourceUri()
        {
            return string.Concat(_configuration.BaseUrl, _configuration.Version, _configuration.ReadResourceUri );
        }

        
    }
}
