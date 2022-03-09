using Foundation.PageContent.Components.Images;
using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.News.ViewModel
{
    public class NewsItemViewModel
    {
        public Guid Id { get; set; }
        public Guid ThumbnailImageId { get; set; }
        public Guid PreviewImageId { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsPublished { get; set; }
        public string Url { get; set; }
        public string ThumbnailImageUrl { get; set; }
        public IEnumerable<SourceViewModel> ThumbnailImages { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
