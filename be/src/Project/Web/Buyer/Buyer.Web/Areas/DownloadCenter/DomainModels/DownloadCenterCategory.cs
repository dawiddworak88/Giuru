using Buyer.Web.Shared.ViewModels.Files;
using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.DownloadCenter.DomainModels
{
    public class DownloadCenterCategory
    {
        public Guid Id { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<DownloadCenterItemCategory> Subcategories { get; set; }
        public IEnumerable<Guid> Files { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
