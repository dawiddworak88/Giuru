namespace Buyer.Web.Shared.DomainModels.Prices
{
    public class Price
    {
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public PriceProduct Product { get; set; }
    }
}
