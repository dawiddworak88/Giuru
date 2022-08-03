using System;
using System.Collections.Generic;

namespace DownloadCenter.Api.ServicesModels.DownloadCenter
{
    public class DownloadCenterCategoryServiceModel
    {
        public Guid Id { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<DownloadCenterSubcategoryServiceModel> Subcategories { get; set; }
        public IEnumerable<Guid> Files { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
