namespace Foundation.ApiExtensions.Models.Request
{
    public class FileRequestModelBase
    {
        public string Id { get; set; }
        public string UploadId { get; set; }
        public byte[] File { get; set; }
        public string Filename { get; set; }
        public int? ChunkNumber { get; set; }
    }
}
