using Foundation.PageContent.ResponseModels.Seo;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Content.DomainModel
{
    public class Content
    {
        public string Title { get; set; }
        public Seo Seo { get; set; }
        public IEnumerable<SharedComponent> SharedComponents { get; set; }
    }
}
