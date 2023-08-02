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
using System.Xml;
using System.Text.Json.Serialization;
using System.Text.Json;

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

        public PictureController(ILogger<PictureController> logger, IWebHostEnvironment webHostEnvironmen, IConfiguration configuration)
        {
            _logger = logger;
            _webHostEnvironmen = webHostEnvironmen;
            _configuration = configuration;
        }

        [HttpGet(template: "Get")]
        public IActionResult Get()
        {
            string myDb1ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            _logger.LogInformation($"--------ConnectionString is {myDb1ConnectionString}");

            List<Info> infos = new List<Info>();

            try
            {
                var options = new DbContextOptionsBuilder<DbContexts>().UseSqlServer(myDb1ConnectionString).Options;

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

                    string jsonString = JsonSerializer.Serialize(image);
                    _logger.LogInformation($"--------JsonSerializer.Serialize is: {jsonString}");

                    infos.Add(new Info
                    {
                        Information = item,
                        dbName = image.FileName,
                        FileSize = image.FileSize,
                        FileType = image.FileType,
                        DbImage = image.Image
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"--------ConnectionString is {ex}");
            }
            //var objFiles = contents.OrderBy(m => m.LastModified);
            return new JsonResult(infos);
        }

        [HttpPost(template: "UploadImage")]
        public async Task<IActionResult> Post([FromForm] IFormFile file, [FromForm] string fileString)
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

                    if (fileString != null)
                    {
                        img.Image = Convert.FromBase64String(fileString.Split(";base64,")[1]);
                        img.FileSize = fileString.Length;
                        img.FileType = fileString.Split(";base64,")[0].Split(":")[1];
                    }

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

        [HttpPost(template: "RemoveImage")]
        public async Task<IActionResult> Delete([FromForm] string imageName)
        {
            string myDb1ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            var options = new DbContextOptionsBuilder<DbContexts>().UseSqlServer(myDb1ConnectionString).Options;

            try
            {
                using (DbContexts db = new DbContexts(options))
                {
                    ImageWork image = db.ImageWork.Where(x => x.FileName.Equals(imageName)).FirstOrDefault();
                    _logger.LogInformation($"--------JsonSerializer.Serialize for remove is: {image}");
                    if (image == null)
                    {
                        return new JsonResult("Not Exist");
                    }

                    var provider = new PhysicalFileProvider(_webHostEnvironmen.WebRootPath);
                    var dir = Path.Combine(Path.Combine(provider.Root, "images"), image.FileName);
                    if (System.IO.File.Exists(dir))
                    {
                        System.IO.File.Delete(dir);
                        db.ImageWork.Remove(image);
                        db.SaveChanges();
                        return new JsonResult("OK");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"--------ConnectionString from catch is {ex}");
            }
            return new JsonResult("No Action");
        }
    }
}