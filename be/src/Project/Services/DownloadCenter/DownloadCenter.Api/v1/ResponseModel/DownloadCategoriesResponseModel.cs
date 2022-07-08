using System;
using System.Collections.Generic;

namespace DownloadCenter.Api.v1.ResponseModel
{
    public class DownloadCategoriesResponseModel
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<DownloadCategoryResponseModel> Categories { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
