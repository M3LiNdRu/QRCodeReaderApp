using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QRCodeReader.Provider.GoQr.Interfaces;
using QRCodeReader.Provider.GoQr.Models;
using RestSharp;

namespace QRCodeReader.Provider.GoQr.Helpers
{
    public class RestSharpQrCodeClientHelper : IGoQRCodeClientHelper
    {
        private static string ERROR_MESSAGE = "Something when wrong";
        private readonly GoQRCodeProviderConfiguration qRCodeProviderConfiguration;
        private readonly RestClient restClient;
        private readonly ILogger<RestSharpQrCodeClientHelper> _logger;

        public RestSharpQrCodeClientHelper(IOptionsMonitor<GoQRCodeProviderConfiguration> options,
            ILogger<RestSharpQrCodeClientHelper> logger)
        {
            qRCodeProviderConfiguration = options.CurrentValue;
            restClient = new RestClient();
            _logger = logger;
        }

        Task<GoQRApiResult> IGoQRCodeClientHelper.PostAsync<T>(string url, T data)
        {
            throw new NotImplementedException();
        }

        async Task<GoQRApiResult> IGoQRCodeClientHelper.PostImageAsync(string url, GoQRCodeFromFileData data)
        {
            var result = new GoQRApiResult();
            restClient.BaseUrl = new Uri(url);
            var request = new RestRequest(Method.POST);
            //request.AddFileBytes("file", ReadFully(data.File), data.Name, "multipart/form-data");
            request.AddFile("file", "./qr-test.png", "multipart/form-data");
            request.AddQueryParameter("MAX_FILE_SIZE", "1048576");

            try
            {
                var response = await restClient.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    result.Data = JsonConvert.DeserializeObject<GoQRCodeApiResponse>(response.Content);
                    result.Success = true;
                }
                else
                {
                    result.Message = "Unable to retrieve info";
                    _logger.LogError(result.Message, data, response);
                }

            } catch (Exception ex)
            {
                _logger.LogError(ex, ERROR_MESSAGE, data);
                result.Message = ERROR_MESSAGE;
            }
            

            return result;
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
