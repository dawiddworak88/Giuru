using System;
using System.Collections.Generic;

namespace DownloadCenter.Api.v1.ResponseModel
{
    public class DownloadCenterCategoriesResponseModel
    {
        public Guid Id { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<DownloadCenterCategoryResponseModel> Categories { get; set; }
        public IEnumerable<Guid> Files { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
