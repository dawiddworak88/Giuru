using Foundation.PageContent.ComponentModels;

namespace Buyer.Web.Areas.Products.ComponentModels
{
    public class PriceComponentModel : ComponentModelBase
    {
        public string CurrencyCode { get; set; }
        public string ExtraPacking { get; set; }
        public string PaletteLoading { get; set; }
        public string Country { get; set; }
        public string DeliveryZipCode { get; set; }
    }
}
