using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using Seller.Portal.Areas.Products.DomainModels;
using Seller.Portal.Shared.ViewModels;

namespace Seller.Portal.Areas.Products.ViewModels
{
    public class ProductPageCatalogViewModel : CatalogBaseViewModel
    {
        public string SkuLabel { get; set; }
        public string NameLabel { get; set; }
        public string LastModifiedDateLabel { get; set; }
        public string CreatedDateLabel { get; set; }
        public PagedResults<IEnumerable<Product>> PagedProducts { get; set; }
    }
}
