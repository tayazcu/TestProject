using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Project.WebApi.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class PictureController : ControllerBase
    {
        private readonly ILogger<PictureController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironmen;

        public PictureController(ILogger<PictureController> logger, IWebHostEnvironment webHostEnvironmen)
        {
            _logger = logger;
            _webHostEnvironmen = webHostEnvironmen;
        }

        [HttpGet(template: "Get")]
        public IActionResult Get()
        {
            var provider = new PhysicalFileProvider(_webHostEnvironmen.WebRootPath);
            var contents = provider.GetDirectoryContents(Path.Combine("images"));
            var objFiles = contents.OrderBy(m => m.LastModified);
            return new JsonResult(objFiles);
        }

        [HttpPost(template: "UploadImage")]
        public async Task<IActionResult> Post([FromForm] IFormFile file)
        {
            try
            {
                var fileName = Path.GetFileName(file.FileName);
                _logger.LogInformation($"--------image {fileName} not uploaded, step 1 - in upliad method");
                await UploadFile(file);
                _logger.LogInformation($"--------image {fileName} not uploaded, step 2 - in upliad method");
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                var fileName = Path.GetFileName(file.FileName);
                _logger.LogInformation($"image {fileName} not uploaded, {ex}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        private async Task<bool> UploadFile(IFormFile ufile)
        {
            if (ufile != null && ufile.Length > 0)
            {
                var fileName = Path.GetFileName(ufile.FileName);

                var provider = new PhysicalFileProvider(_webHostEnvironmen.WebRootPath);
                var ss = provider.Root;
                var sd = Path.Combine(ss, "images");
                var sddd = Path.Combine(sd, fileName);

                //var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images", fileName);
                using (var fileStream = new FileStream(sddd, FileMode.Create))
                {
                    await ufile.CopyToAsync(fileStream);
                }
                _logger.LogInformation($"image {fileName} uploaded");
                return true;
            }
            return false;
        }
    }
}