using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Project.WebApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Project.WebApi.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class PictureController : ControllerBase
    {
        private readonly ILogger<PictureController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironmen;
        private readonly IConfiguration _configuration;

        public PictureController(ILogger<PictureController> logger, IWebHostEnvironment webHostEnvironmen , IConfiguration configuration)
        {
            _logger = logger;
            _webHostEnvironmen = webHostEnvironmen;
            _configuration = configuration;
        }

        [HttpGet(template: "Get")]
        public IActionResult Get()
        {
            string myDb1ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            var options = new DbContextOptionsBuilder<DbContexts>().UseSqlServer(myDb1ConnectionString).Options;

            List<Info> infos = new List<Info>();

            var provider = new PhysicalFileProvider(_webHostEnvironmen.WebRootPath);
            var contents = provider.GetDirectoryContents(Path.Combine("images"));
            foreach (var item in contents)
            {
                ImageWork image = null;
                using (DbContexts db = new DbContexts(options))
                {
                    image = db.ImageWork.Where(x => x.FileName.Equals(item.Name)).FirstOrDefault();
                    if (image == null)
                    {
                        image = new ImageWork
                        {
                            FileName = "Not Exist"
                        };
                    }
                }

                infos.Add(new Info
                {
                    Information = item,
                    dbName = image.FileName,
                });
            }
            //var objFiles = contents.OrderBy(m => m.LastModified);
            return new JsonResult(infos);
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

                string myDb1ConnectionString = _configuration.GetConnectionString("DefaultConnection");
                var options = new DbContextOptionsBuilder<DbContexts>().UseSqlServer(myDb1ConnectionString).Options;
                using (DbContexts db = new DbContexts(options))
                {
                    ImageWork img = new ImageWork();
                    img.FileName = fileName;
                    db.ImageWork.Add(img);
                    db.SaveChanges();
                    _logger.LogInformation($"--------image {fileName} not uploaded, step 3 - added to database");
                }

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