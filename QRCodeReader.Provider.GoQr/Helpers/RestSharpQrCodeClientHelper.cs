using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QRCodeReader.Provider.GoQr.Interfaces;
using QRCodeReader.Provider.GoQr.Models;
using RestSharp;

namespace QRCodeReader.Provider.GoQr.Helpers
{
    public class RestSharpQrCodeClientHelper : IGoQRCodeClientHelper
    {
        private readonly GoQRCodeProviderConfiguration qRCodeProviderConfiguration;
        private readonly RestClient restClient;

        public RestSharpQrCodeClientHelper(IOptionsMonitor<GoQRCodeProviderConfiguration> options)
        {
            qRCodeProviderConfiguration = options.CurrentValue;
            restClient = new RestClient();
        }

        Task<GoQRCodeApiResponse> IGoQRCodeClientHelper.PostAsync<T>(string url, T data)
        {
            throw new NotImplementedException();
        }

        async Task<GoQRCodeApiResponse> IGoQRCodeClientHelper.PostImageAsync(string url, GoQRCodeFromFileData data)
        {
            restClient.BaseUrl = new Uri(url);

            var request = new RestRequest(Method.POST);
            //request.AddFileBytes("file", ReadFully(data.File), data.Name, "multipart/form-data");
            request.AddFile("file", "./qr-test.png", "multipart/form-data");
            request.AddQueryParameter("MAX_FILE_SIZE", "1048576");

            //Sending request to Post Method
            IRestResponse response = await restClient.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<GoQRCodeApiResponse>(response.Content);

                return result;
            }


            return null;
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
