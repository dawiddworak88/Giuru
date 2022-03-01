using Foundation.ApiExtensions.Models.Request;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.News.ApiRequestModels
{
    public class NewsRequestModel : RequestModelBase
    {
        public Guid? HeroImageId { get; set; }
        public Guid? CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public bool IsNew { get; set; }
        public bool IsPublished { get; set; }
        public IEnumerable<ListItemRequestModel> Images { get; set; }
        public IEnumerable<ListItemRequestModel> Files { get; set; }
    }
}
