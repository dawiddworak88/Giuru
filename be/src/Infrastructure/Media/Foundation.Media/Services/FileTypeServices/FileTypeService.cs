namespace Foundation.Media.Services.FileTypeServices
{
    public class FileTypeService : IFileTypeService
    {
        public FileTypeService()
        { }
        
        public bool IsImage(string mimeType)
        {
            return mimeType.StartsWith("image");
        }
    }
}
