using System;
using Microsoft.Extensions.Options;
using QRCodeReader.Provider.GoQr.Models;

namespace QRCodeReader.Console
{
    internal class OptionsMonitorGoQRConfiguration : IOptionsMonitor<GoQRCodeProviderConfiguration>
    {
        private readonly GoQRCodeProviderConfiguration _config;
        public OptionsMonitorGoQRConfiguration()
        {
            _config = new GoQRCodeProviderConfiguration
            {
                BaseUrl = "http://api.qrserver.com",
                ReadResourceUri = "read-qr-code",
                Version = "v1"
            };
        }

        GoQRCodeProviderConfiguration IOptionsMonitor<GoQRCodeProviderConfiguration>.CurrentValue => _config;

        GoQRCodeProviderConfiguration IOptionsMonitor<GoQRCodeProviderConfiguration>.Get(string name)
        {
            throw new NotImplementedException();
        }

        IDisposable IOptionsMonitor<GoQRCodeProviderConfiguration>.OnChange(Action<GoQRCodeProviderConfiguration, string> listener)
        {
            throw new NotImplementedException();
        }
    }
}