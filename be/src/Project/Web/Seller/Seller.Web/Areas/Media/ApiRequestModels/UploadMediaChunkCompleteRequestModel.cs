using System;

namespace Seller.Web.Areas.Media.ApiRequestModels
{
    public class UploadMediaChunkCompleteRequestModel
    {
        public Guid? Id { get; set; }
        public Guid UploadId { get; set; }
        public string Filename { get; set; }
    }
}
