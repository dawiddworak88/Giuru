using Foundation.PageContent.ComponentModels;
using System;

namespace Buyer.Web.Areas.Products.ComponentModels
{
    public class CategoryComponentModel : ComponentModelBase
    {
        public Guid? CategoryId { get; set; }
        public string SearchTerm { get; set; }
    }
}
