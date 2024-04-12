using Foundation.PageContent.ResponseModels.Seo;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Content.DomainModel
{
    public class Slug
    {
        public string Title { get; set; }
        public Seo Seo { get; set; }
        public IEnumerable<BlockPage> Blocks { get; set; }
    }
}
