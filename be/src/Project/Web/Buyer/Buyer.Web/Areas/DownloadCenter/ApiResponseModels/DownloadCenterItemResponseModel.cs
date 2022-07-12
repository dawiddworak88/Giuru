using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.DownloadCenter.ApiResponseModels
{
    public class DownloadCenterItemResponseModel
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<DownloadCenterItemCategoryResponseModel> Categories { get; set; }
        public int? Order { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
