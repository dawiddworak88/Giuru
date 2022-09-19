using System;
using System.Collections.Generic;

namespace DownloadCenter.Api.ServicesModels.DownloadCenter
{
    public class DownloadCenterCategoryItemServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<DownloadCenterSubcategoryServiceModel> Subcategories { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
