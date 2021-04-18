using Buyer.Web.Shared.ViewModels.Catalogs;
using System;

namespace Buyer.Web.Areas.Products.ViewModels.Brands
{
    public class BrandCatalogViewModel : CatalogViewModel
    {
        public Guid? BrandId { get; set; }
    }
}
