using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.DownloadCenter.ApiResponseModels
{
    public class DownloadCenterCategoryResponseModel
    {
        public Guid Id { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<Category2> Categories { get; set; }
        public IEnumerable<Guid> Files { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
