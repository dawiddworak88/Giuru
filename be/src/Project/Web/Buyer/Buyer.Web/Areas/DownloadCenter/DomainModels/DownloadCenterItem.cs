using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.DownloadCenter.DomainModels
{
    public class DownloadCenterItem
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<DownloadCenterItemCategory> Categories { get; set; }
        public int? Order { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
