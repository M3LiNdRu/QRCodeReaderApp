using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using QRCodeReader.Core.Interfaces;
using QRCodeReader.Core.Models;

namespace QRCodeReader.Core.Helpers
{
    public class GoQRCodeClientHelper : IGoQRCodeClientHelper
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerSettings _jsonSerializerOptions;

        public GoQRCodeClientHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonSerializerOptions = new JsonSerializerSettings {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

        }

        async Task<GoQRCodeApiResponse> IGoQRCodeClientHelper.PostAsync<T>(string url, T data)
        {
            var body = JsonConvert.SerializeObject(data, _jsonSerializerOptions);
            var stringContent = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, stringContent);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GoQRCodeApiResponse>(responseString);

            return result;
        }
    }
}
