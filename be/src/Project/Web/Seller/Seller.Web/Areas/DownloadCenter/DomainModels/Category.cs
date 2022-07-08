using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.DownloadCenter.DomainModels
{
    public class Category
    {
        public Guid Id { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }
        public string Name { get; set; }
        public IEnumerable<Guid> Files { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
