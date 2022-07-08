using System;

namespace Seller.Web.Areas.DownloadCenter.DomainModels
{
    public class DownloadCenter
    {
        public Guid Id { get; set; }
        public Guid? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? Order { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
