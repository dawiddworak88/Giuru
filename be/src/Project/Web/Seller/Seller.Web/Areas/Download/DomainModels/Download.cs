using System;

namespace Seller.Web.Areas.Download.DomainModels
{
    public class Download
    {
        public Guid Id { get; set; }
        public Guid? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? Order { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
