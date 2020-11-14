using System.Threading.Tasks;
using QRCodeReader.Provider.GoQr.Models;

namespace QRCodeReader.Provider.GoQr.Interfaces
{
    public interface IGoQRCodeClientHelper
    {
        Task<GoQRCodeApiResponse> PostAsync<T>(string url, T data);
        Task<GoQRCodeApiResponse> PostImageAsync(string url, GoQRCodeFromFileData data);
    }
}
