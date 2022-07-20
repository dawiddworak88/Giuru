using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.DownloadCenter.ApiResponseModels
{
    public class DownloadCenterItemResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<DownloadCenterItemCategoryResponseModel> Categories { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
