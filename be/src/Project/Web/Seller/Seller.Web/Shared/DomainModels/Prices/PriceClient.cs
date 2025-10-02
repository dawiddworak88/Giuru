using System;

namespace Seller.Web.Shared.DomainModels.Prices
{
    public class PriceClient
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string ExtraPacking { get; set; }
        public string PaletteLoading { get; set; }
        public string CurrencyCode { get; set; }
        public string Country { get; set; }
        public string DeliveryZipCode { get; set; }
    }
}
