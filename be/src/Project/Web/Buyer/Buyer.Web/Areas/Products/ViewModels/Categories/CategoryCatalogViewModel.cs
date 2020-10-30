using Buyer.Web.Shared.Catalogs.ViewModels;
using System;

namespace Buyer.Web.Areas.Products.ViewModels.Categories
{
    public class CategoryCatalogViewModel : CatalogViewModel
    {
        public Guid? CategoryId { get; set; }
    }
}
