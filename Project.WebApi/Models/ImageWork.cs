namespace Project.WebApi.Models
{
    public class ImageWork
    {
        public ImageWork()
        {
                
        }

        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] Image { get; set; }
        public string FileType { get; set; }
        public int FileSize { get; set; }
    }
}
