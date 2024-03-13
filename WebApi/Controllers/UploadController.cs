using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Services.Common;

namespace WebApi.Controllers
{

    public class UploadController : ApiControllerBase
    {
        private readonly UploadService _upload;

        public UploadController(UploadService upload)
        {
            _upload = upload;
        }

        [HttpPost]
        public async Task<IActionResult> SaveFile(List<IFormFile> files)
        {

            var tokens = await _upload.SaveFiles(files);
            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(tokens);
        }
    }
}
