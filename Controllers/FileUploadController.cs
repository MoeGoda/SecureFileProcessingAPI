using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureFileProcessingAPI.Services;

namespace SecureFileProcessingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : Controller
    {
        private readonly BlobStorageService _blobStorageService;

        public FileUploadController(BlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        [HttpPost("upload")]
        [Authorize]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var fileUrl = await _blobStorageService.UploadFileAsync(file);

            var sasToken = _blobStorageService.GenerateSasToken(file.FileName);

            return Ok(new { FileUrl = fileUrl, SasToken = sasToken });
        }
    }
}
