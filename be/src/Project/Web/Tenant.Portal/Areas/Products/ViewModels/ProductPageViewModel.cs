using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using Tenant.Portal.Areas.Products.DomainModels;
using Tenant.Portal.Shared.ViewModels;

namespace Tenant.Portal.Areas.Products.ViewModels
{
    public class ProductPageViewModel : CatalogBasePageViewModel
    {
        public string SkuLabel { get; set; }
        public string NameLabel { get; set; }
        public string LastModifiedDateLabel { get; set; }
        public string CreatedDateLabel { get; set; }
        public PagedResults<IEnumerable<Product>> PagedProducts { get; set; }
    }
}
