namespace Foundation.Pricing.Services
{
    // Per-app adapter bound to a single product, over that app's own Product/ProductAttribute types.
    public interface IProductAttributeReader
    {
        string GetFirstAvailableAttributeValue(string possibleKeys);

        string GetSleepAreaSize();

        string GetSize();
    }
}
