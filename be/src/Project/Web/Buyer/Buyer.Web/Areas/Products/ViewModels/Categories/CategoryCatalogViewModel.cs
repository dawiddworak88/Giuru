using Buyer.Web.Shared.ViewModels.Catalogs;
using System;

namespace Buyer.Web.Areas.Products.ViewModels.Categories
{
    public class CategoryCatalogViewModel : CatalogViewModel
    {
        public Guid? CategoryId { get; set; }
    }
}
