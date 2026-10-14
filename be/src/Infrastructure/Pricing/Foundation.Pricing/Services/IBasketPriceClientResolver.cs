namespace Foundation.Pricing.Services
{
    /// <summary>
    /// The resolver the basket save and upload paths use to reprice. It keeps the discount code the basket
    /// coordinator hands it and never validates it again: the coordinator has already decided which code is
    /// in force - including whether a code that is already stored on the basket is revalidated
    /// (see <see cref="Baskets.BasketDiscountCodeCoordinator"/>).
    /// Fresh product price reads use <see cref="IPriceClientResolver"/>, which treats a stored code the same way.
    /// </summary>
    public interface IBasketPriceClientResolver : IPriceClientResolver
    {
    }
}
