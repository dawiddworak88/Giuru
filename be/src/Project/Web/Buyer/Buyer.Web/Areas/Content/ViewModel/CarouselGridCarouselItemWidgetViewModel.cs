using Foundation.PageContent.Components.Images;
using System.Collections.Generic;
using System;

namespace Buyer.Web.Areas.Content.ViewModel
{
    public class CarouselGridCarouselItemWidgetViewModel
    {
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string ImageAlt { get; set; }
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public string Subtitle { get; set; }
        public string Ean { get; set; }
        public string OutletTitle { get; set; }
        public double? AvailableQuantity { get; set; }
        public double? AvailableOutletQuantity { get; set; }
        public DateTime? ExpectedDelivery { get; set; }
        public DateTime? CreatedDate { get; set; }
        public IEnumerable<SourceViewModel> Sources { get; set; }
    }
}
