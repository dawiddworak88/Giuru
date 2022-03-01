using System;

namespace Seller.Web.Areas.News.DomainModels
{
    public class NewsItem
    {
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
