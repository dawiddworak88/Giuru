using Foundation.PageContent.Components.ListItems.ViewModels;
using System.Collections.Generic;

namespace Seller.Web.Areas.Products.ViewModels
{
    public class ProductCardModalViewModel
    {
        public string Title { get; set; }
        public string NameLabel { get; set; }
        public string DisplayNameLabel { get; set; }
        public string DefinitionLabel { get; set; }
        public string SaveText { get; set; }
        public string CancelText { get; set; }
        public string InputTypeLabel { get; set; }
        public IEnumerable<ProductCardModalInputTypeViewModel> InputTypes { get; set; }
        public IEnumerable<ListItemViewModel> DefinitionsOptions { get; set; }
    }
}
