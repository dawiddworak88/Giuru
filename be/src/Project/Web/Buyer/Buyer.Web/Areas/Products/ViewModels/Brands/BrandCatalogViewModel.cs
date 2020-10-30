using Buyer.Web.Shared.Catalogs.ViewModels;
using System;

namespace Buyer.Web.Areas.Products.ViewModels.Brands
{
    public class BrandCatalogViewModel : CatalogViewModel
    {
        public Guid? BrandId { get; set; }
    }
}
