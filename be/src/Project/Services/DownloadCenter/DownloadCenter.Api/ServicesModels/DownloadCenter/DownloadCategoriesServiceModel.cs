using System;
using System.Collections.Generic;

namespace DownloadCenter.Api.ServicesModels.DownloadCenter
{
    public class DownloadCategoriesServiceModel
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<DownloadCategoryServiceModel> Categories { get; set; }
        public Guid CategoryFileId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
