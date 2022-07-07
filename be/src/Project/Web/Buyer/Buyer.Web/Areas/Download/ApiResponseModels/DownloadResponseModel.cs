using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Download.ApiResponseModels
{
    public class DownloadResponseModel
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<DownloadCategoryResponseModel> Categories { get; set; }
        public int? Order { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
