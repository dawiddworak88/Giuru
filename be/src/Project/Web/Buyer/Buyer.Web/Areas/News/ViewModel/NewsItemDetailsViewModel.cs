using Foundation.PageContent.Components.Images;
using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.News.ViewModel
{
    public class NewsItemDetailsViewModel
    {

        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string HeroImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public IEnumerable<SourceViewModel> HeroImages { get; set; }

    }
}
