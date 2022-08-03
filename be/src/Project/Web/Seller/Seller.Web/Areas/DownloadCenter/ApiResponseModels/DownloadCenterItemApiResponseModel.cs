using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.DownloadCenter.ApiResponseModels
{
    public class DownloadCenterItemApiResponseModel
    {
        public Guid Id { get; set; }
        public string Filename { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
