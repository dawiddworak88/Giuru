using System;

namespace Seller.Web.Areas.Media.ApiResponseModels
{
    public class MediaItemResponseModel
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
        public string MimeType { get; set; }
        public Guid? MediaItemId { get; set; }
        public Guid? MediaItemVersionId { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
