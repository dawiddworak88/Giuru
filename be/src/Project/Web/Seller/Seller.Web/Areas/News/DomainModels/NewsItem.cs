using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.News.DomainModels
{
    public class NewsItem
    {
        public Guid Id { get; set; }
        public Guid? ThumbnailImageId { get; set; }
        public Guid? PreviewImageId { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string CategoryName { get; set; }
        public bool IsPublished { get; set; }
        public IEnumerable<Guid> ClientGroupIds { get; set; }
        public IEnumerable<Guid> Files { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
