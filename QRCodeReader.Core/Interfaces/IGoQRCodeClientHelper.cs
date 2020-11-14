using System.Threading.Tasks;
using QRCodeReader.Core.Models;

namespace QRCodeReader.Core.Interfaces
{
    public interface IGoQRCodeClientHelper
    {
        Task<GoQRCodeApiResponse> PostAsync<T>(string url, T data);
        Task<GoQRCodeApiResponse> PostImageAsync(string url, GoQRCodeFromFileData data);
    }
}
