using Seller.Web.Shared.DomainModels.Products;
using Seller.Web.Shared.ViewModels;
using System;

namespace Seller.Web.Areas.Products.ViewModels
{
    public class ProductAttributePageViewModel : BasePageViewModel
    {
        public Guid? Id { get; set; }
        public ProductAttributeFormViewModel ProductAttributeForm { get; set; }
        public CatalogViewModel<ProductAttributeItem> Catalog { get; set; }
    }
}
