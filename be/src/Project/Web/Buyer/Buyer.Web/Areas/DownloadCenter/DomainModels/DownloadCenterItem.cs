using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.DownloadCenter.DomainModels
{
    public class DownloadCenterItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<DownloadCenterItemCategory> Categories { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
