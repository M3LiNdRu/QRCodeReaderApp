using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using QRCodeReader.Core.Interfaces;
using QRCodeReader.Core.Models;
using QRCodeReader.Provider.GoQr.Concrete;
using QRCodeReader.Provider.GoQr.Helpers;
using QRCodeReader.Provider.GoQr.Interfaces;
using QRCodeReader.Provider.GoQr.Models;

namespace QRCodeReader.Console
{
    class Program
    {
        private static IServiceProvider serviceProvider;
        private static IQRCodeProvider qRCodeProvider;

        static async Task Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddOptions();
            //serviceCollection.Configure<GoQRCodeProviderConfiguration>((c) =>
            //{
            //    c = configuration.GetSection("GoQRCodeProvider").Get<GoQRCodeProviderConfiguration>();
            //});
            serviceCollection.AddSingleton<IOptionsMonitor<GoQRCodeProviderConfiguration>, OptionsMonitorGoQRConfiguration>();
            serviceCollection.AddSingleton<IQRCodeProvider, GoQRCodeProvider>();

            serviceCollection.AddHttpClient<IGoQRCodeClientHelper, GoQRCodeClientHelper>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy());

            serviceProvider = serviceCollection.BuildServiceProvider();

            var provider = serviceProvider.GetService<IQRCodeProvider>();


            var data = await LoadQRFromLocalFile(@"./qr-test.png");
            var result = await provider.Read(data);


            System.Console.WriteLine(result.Data);
        }

        private static async Task<QRCodeFromFile> LoadQRFromLocalFile(string filePath)
        {
            var result = new QRCodeFromFile();
            var file = await File.ReadAllBytesAsync(filePath);

            result.Name = filePath;
            result.Stream = new MemoryStream(file);

            return result;
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                            retryAttempt)));
        }
    }
}
