using Foundation.PageContent.Components.Images;
using System;
using System.Collections.Generic;

namespace Foundation.PageContent.Components.CarouselGrids.ViewModels
{
    public class CarouselGridCarouselItemViewModel
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string ImageAlt { get; set; }
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public string Subtitle { get; set; }
        public string Ean { get; set; }
        public int? AvailableQuantity { get; set; }
        public int? AvailableOutletQuantity { get; set; }
        public DateTime? ExpectedDelivery { get; set; }
        public DateTime? CreatedDate { get; set; }
        public IEnumerable<SourceViewModel> Sources { get; set; }
        public IEnumerable<CarouselGridProductAttributesViewModel> Attributes { get; set; }
        public IEnumerable<ImageVariantViewModel> Images { get; set; }
    }
}
