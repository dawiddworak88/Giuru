using System;
using System.Collections.Generic;

namespace DownloadCenter.Api.v1.ResponseModel
{
    public class DownloadCenterItemCategoriesResponseModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<DownloadCenterSubcategoryResponseModel> Subcategories { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
