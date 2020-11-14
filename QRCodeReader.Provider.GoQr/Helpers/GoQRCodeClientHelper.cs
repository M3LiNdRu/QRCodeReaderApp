using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using QRCodeReader.Provider.GoQr.Interfaces;
using QRCodeReader.Provider.GoQr.Models;

namespace QRCodeReader.Provider.GoQr.Helpers
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

        async Task<GoQRCodeApiResponse> IGoQRCodeClientHelper.PostImageAsync(string url, GoQRCodeFromFileData data) 
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent("1048576"), "MAX_FILE_SIZE", "MAX_FILE_SIZE");
            content.Add(new StreamContent(data.File), "file", data.Name);
            //content.Add(new ByteArrayContent(ReadFully(data.File)), "file", data.Name);

            var response = await _httpClient.PostAsync(url, content);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GoQRCodeApiResponse>(responseString);

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
