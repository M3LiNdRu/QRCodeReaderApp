using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddJsonFile($"appsettings.json", true, true);
                    builder.AddEnvironmentVariables();

                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<GoQRCodeProviderConfiguration>(hostContext.Configuration.GetSection("GoQRCodeProvider"));
                    services.AddLogging(configure => configure.AddConsole());
                    services.AddSingleton<IQRCodeProvider, GoQRCodeProvider>();
                    services.AddHttpClient<IGoQRCodeClientHelper, GoQRCodeClientHelper>()
                        .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                        .AddPolicyHandler(GetRetryPolicy());

                }).UseConsoleLifetime();

            var host = builder.Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;

                var provider = serviceProvider.GetService<IQRCodeProvider>();


                var data = await LoadQRFromLocalFile(@"./qr-test.png");
                var result = await provider.Read(data);


                System.Console.WriteLine(result.Data);
            }

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
