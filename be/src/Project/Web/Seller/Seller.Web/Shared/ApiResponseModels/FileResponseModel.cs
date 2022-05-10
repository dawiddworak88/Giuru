using Foundation.ApiExtensions.Models.Response;
using System;

namespace Seller.Web.Shared.ApiResponseModels
{
    public class FileResponseModel : BaseResponseModel
    {
        public string Name { get; set; }
        public string Filename { get; set; }
        public string Description { get; set; }
        public bool IsProtected { get; set; }
        public long Size { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
