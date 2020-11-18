using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using QRCodeReader.Provider.GoQr.Interfaces;
using QRCodeReader.Provider.GoQr.Models;

namespace QRCodeReader.Provider.GoQr.Helpers
{
    public class GoQRCodeClientHelper : IGoQRCodeClientHelper
    {
        private static string ERROR_MESSAGE = "Something when wrong";
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerSettings _jsonSerializerOptions;
        private readonly ILogger<GoQRCodeClientHelper> _logger;

        public GoQRCodeClientHelper(HttpClient httpClient, ILogger<GoQRCodeClientHelper> logger)
        {
            _httpClient = httpClient;
            _jsonSerializerOptions = new JsonSerializerSettings {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            _logger = logger;
        }

        async Task<GoQRApiResult> IGoQRCodeClientHelper.PostAsync<T>(string url, T data)
        {
            throw new NotImplementedException();
        }

        async Task<GoQRApiResult> IGoQRCodeClientHelper.PostImageAsync(string url, GoQRCodeFromFileData data) 
        {
            var result = new GoQRApiResult();
            var content = new MultipartFormDataContent();
            content.Add(new StringContent("1048576"), "MAX_FILE_SIZE", "MAX_FILE_SIZE");
            content.Add(new StreamContent(data.File), "file", data.Name);

            try
            {
                using var response = await _httpClient.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    result.Data = await ParseResponse<GoQRCodeApiResponse>(response);
                    result.Success = true;
                }
                else {
                    result.Message = "Unable to retrieve data from GoQR";
                    _logger.LogError(result.Message, data, response);
                }
                
            } catch (Exception ex)
            {
                _logger.LogError(ex, ERROR_MESSAGE, data);
                result.Message = ERROR_MESSAGE;
            }

            return result;
        }

        async Task<T> ParseResponse<T>(HttpResponseMessage response)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseString);
        }
    }
}
