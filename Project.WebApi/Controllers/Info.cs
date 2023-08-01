using Microsoft.Extensions.FileProviders;

namespace Project.WebApi.Controllers
{
    public class Info
    {
        public IFileInfo Information { get; set; }
        public string dbName { get; set; }
        public string FileType { get; set; }
        public int FileSize { get; set; }
        public byte[] DbImage { get; set; }
        public string DataUrl { get; set; } = string.Empty;
    }
}