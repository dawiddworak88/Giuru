namespace Foundation.ApiExtensions.Models.Request
{
    public class FileRequestModelBase
    {
        public byte[] File { get; set; }
        public string Filename { get; set; }
        public int? ChunkNumber { get; set; }
        public string Id { get; set; }
    }
}
