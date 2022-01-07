using System;

namespace Buyer.Web.Shared.ViewModels.Catalogs
{
    public class CatalogItemViewModel
    {
        public Guid Id { get; set; }
        public string Sku { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string ImageAlt { get; set; }
        public bool IsNew { get; set; }
        public bool InStock { get; set; }
        public int? AvailableQuantity { get; set; }
        public string BrandName { get; set; }
        public string BrandUrl { get; set; }
    }
}
