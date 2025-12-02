using System;

namespace Foundation.ApiExtensions.Models.Request
{
    public class FileChunksSaveCompleteRequestModel
    {
        public Guid? Id { get; set; }
        public Guid UploadId { get; set; }
        public string Filename { get; set; }
    }
}
