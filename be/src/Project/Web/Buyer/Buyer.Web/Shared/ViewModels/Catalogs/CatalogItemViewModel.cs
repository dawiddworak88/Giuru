using Foundation.PageContent.Components.Images;
using System;
using System.Collections.Generic;

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
        public bool InOutlet { get; set; }
        public bool CanOrder { get; set; }
        public double? AvailableQuantity { get; set; }
        public double? AvailableOutletQuantity { get; set; }
        public DateTime? ExpectedDelivery { get; set; }
        public string BrandName { get; set; }
        public string BrandUrl { get; set; }
        public string OutletTitle { get; set; }
        public string OutletDescription { get; set; }
        public IEnumerable<Guid> Images { get; set; }
        public IEnumerable<SourceViewModel> Sources { get; set; }
        public string ProductAttributes { get; set; }
        public int CompletionDate { get; set; }
    }
}
