using Microsoft.Extensions.FileProviders;

namespace Project.WebApi.Controllers
{
    public class Info
    {
        public IFileInfo Information { get; set; }
        public string dbName { get; set; }
    }
}