using System;

namespace Seller.Web.Areas.Media.ApiResponseModels
{
    public class MediaApiResponseModel
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string ImageUrl { get; set; }
        public Guid? MediaItemId { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
