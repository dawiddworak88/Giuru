namespace Foundation.Pricing.Services
{
    public interface IPriceableBasketLine
    {
        double Quantity { get; }
        double StockQuantity { get; }
        double OutletQuantity { get; }
        decimal? UnitPrice { get; set; }
        decimal? Price { get; set; }
        string Currency { get; set; }
    }
}
