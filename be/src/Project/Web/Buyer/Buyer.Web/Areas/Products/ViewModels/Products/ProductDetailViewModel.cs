using Buyer.Web.Shared.DomainModels.Baskets;
using Buyer.Web.Shared.ViewModels.Files;
using Buyer.Web.Shared.ViewModels.Images;
using Buyer.Web.Shared.ViewModels.Sidebar;
using Foundation.PageContent.Components.CarouselGrids.ViewModels;
using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Products.ViewModels.Products
{
    public class ProductDetailViewModel
    {
        public Guid? BasketId { get; set; }
        public string Title { get; set; }
        public Guid? ProductId { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsProductVariant { get; set; }
        public string SignInUrl { get; set; }
        public string SkuLabel { get; set; }
        public string Sku { get; set; }
        public string ByLabel { get; set; }
        public string BrandName { get; set; }
        public string BrandUrl { get; set; }
        public string PricesLabel { get; set; }
        public string ProductInformationLabel { get; set; }
        public string SignInToSeePricesLabel { get; set; }
        public string SuccessfullyAddedProduct { get; set; }
        public bool InStock { get; set; }
        public int? AvailableQuantity { get; set; }
        public DateTime? ExpectedDelivery { get; set; }
        public bool InOutlet { get; set; }
        public int? AvailableOutletQuantity { get; set; }
        public DateTime? ExpectedOutletDelivery { get; set; }
        public string ExpectedDeliveryLabel { get; set; }
        public string RestockableInDaysLabel { get; set; }
        public int? RestockableInDays { get; set; }
        public string DescriptionLabel { get; set; }
        public string Description { get; set; }
        public string InStockLabel { get; set; }
        public string BasketLabel { get; set; }
        public string UpdateBasketUrl { get; set; }
        public string AddedProduct { get; set; }
        public SidebarViewModel Sidebar { get; set; }
        public FilesViewModel Files { get; set; }
        public IEnumerable<BasketItem> OrderItems { get; set; }
        public IEnumerable<ImageViewModel> Images { get; set; }
        public IEnumerable<ProductFeatureViewModel> Features { get; set; }
        public IEnumerable<CarouselGridItemViewModel> ProductVariants { get; set; }
    }
}
