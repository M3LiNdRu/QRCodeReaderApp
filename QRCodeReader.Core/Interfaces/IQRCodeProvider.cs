using System.Threading.Tasks;
using QRCodeReader.Core.Models;

namespace QRCodeReader.Core.Interfaces
{
    public interface IQRCodeProvider
    {
        Task<QRCodeData> Read(QRCodeFromFile data);
    }
}
