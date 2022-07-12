using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.DownloadCenter.DomainModels
{
    public class DownloadCategory2
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<DownloadCategory> Categories { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
