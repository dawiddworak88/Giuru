using System;

namespace Buyer.Web.Areas.News.DomainModels
{
    public class NewsItemFile
    {
        public Guid Id { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
