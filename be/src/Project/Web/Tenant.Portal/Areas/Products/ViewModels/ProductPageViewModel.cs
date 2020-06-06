using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using Tenant.Portal.Areas.Products.DomainModels;
using Tenant.Portal.Shared.ViewModels;

namespace Tenant.Portal.Areas.Products.ViewModels
{
    public class ProductPageViewModel : CatalogBasePageViewModel
    {
        public PagedResults<IEnumerable<Product>> Products { get; set; }
    }
}
