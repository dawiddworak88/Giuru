namespace Foundation.ApiExtensions.Models.Request
{
    public class FileRequestModelBase
    {
        public string Language { get; set; }
        public byte[] File { get; set; }
        public string Filename { get; set; }
    }
}
