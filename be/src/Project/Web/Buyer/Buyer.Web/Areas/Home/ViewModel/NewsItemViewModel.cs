using System;

namespace Buyer.Web.Areas.Home.ViewModel
{
    public class NewsItemViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string CategoryName { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
