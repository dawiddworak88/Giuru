using Foundation.ApiExtensions.Models.Request;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.News.ApiRequestModels
{
    public class NewsApiRequestModel : RequestModelBase
    {
        public Guid? ThumbnailImageId { get; set; }
        public Guid? PreviewImageId { get; set; }
        public Guid? CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public bool IsPublished { get; set; }
        public IEnumerable<Guid> Files { get; set; }
    }
}
