namespace Seller.Web.Shared.Services.Prices
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