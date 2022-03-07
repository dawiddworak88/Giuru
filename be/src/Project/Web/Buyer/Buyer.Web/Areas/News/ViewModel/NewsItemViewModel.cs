using Foundation.PageContent.Components.Images;
using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.News.ViewModel
{
    public class NewsItemViewModel
    {
        public Guid Id { get; set; }
        public Guid ThumbImageId { get; set; }
        public Guid HeroImageId { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsPublished { get; set; }
        public string Url { get; set; }
        public string ThumbImageUrl { get; set; }
        public IEnumerable<SourceViewModel> ThumbImages { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
