using Foundation.PageContent.Components.Images;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Products.ViewModels.Products
{
    public class ProductMediaItemViewModel
    {
        public string MediaSrc { get; set; }
        public string MediaAlt { get; set; }
        public string MimeType { get; set; }
        public IEnumerable<SourceViewModel> Sources { get; set; }
    }
}
