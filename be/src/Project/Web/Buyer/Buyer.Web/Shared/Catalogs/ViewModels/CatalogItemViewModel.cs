using System;

namespace Buyer.Web.Shared.Catalogs.ViewModels
{
    public class CatalogItemViewModel
    {
        public Guid Id { get; set; }
        public string Sku { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string ImageAlt { get; set; }
        public bool InStock { get; set; }
        public string BrandName { get; set; }
        public string BrandUrl { get; set; }
    }
}
