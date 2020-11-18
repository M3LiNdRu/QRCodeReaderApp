using System.Threading.Tasks;
using QRCodeReader.Provider.GoQr.Models;

namespace QRCodeReader.Provider.GoQr.Interfaces
{
    public interface IGoQRCodeClientHelper
    {
        Task<GoQRApiResult> PostAsync<T>(string url, T data);
        Task<GoQRApiResult> PostImageAsync(string url, GoQRCodeFromFileData data);
    }
}
