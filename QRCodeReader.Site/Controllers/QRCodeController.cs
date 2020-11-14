using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRCodeReader.Core.Interfaces;
using QRCodeReader.Core.Models;

namespace QRCodeReader.Site.Controllers
{
    [Route("api/qrcode")]
    public class QRCodeController : Controller
    {
        private readonly IQRCodeProvider _qRCodeProvider;

        public QRCodeController(IQRCodeProvider provider)
        {
            _qRCodeProvider = provider;
        }

        // GET: api/qrcode
        [HttpGet]
        public string Get()
        {
            return "It works :)";
        }

        // POST api/qrcode
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] IFormFile formFile)
        {
            try
            {
                if (formFile.Length > 0)
                {
                    using var stream = new MemoryStream();
                    await formFile.CopyToAsync(stream);
                    var data = new QRCodeFromFile
                    {
                        Name = formFile.FileName,
                        Stream = stream,
                        Size = formFile.Length
                    };
                    var result = await _qRCodeProvider.Read(data);

                    return Ok(result);
                }

                return Ok(new { Message = "No file has been uploaded." });            

            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }
    }
}
