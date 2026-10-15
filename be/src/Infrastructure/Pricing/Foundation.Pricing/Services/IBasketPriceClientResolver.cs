namespace Foundation.Pricing.Services
{
    /// <summary>
    /// The resolver the basket save and upload paths use to reprice. It keeps the discount code the basket
    /// coordinator hands it and never validates it again: the coordinator has already decided which code this
    /// save is priced with - including a stored code it keeps while its applicability cannot be determined
    /// (see <see cref="Baskets.BasketDiscountCodeCoordinator"/>).
    /// Fresh product price reads use <see cref="IPriceClientResolver"/>, which validates the code it is given.
    /// </summary>
    public interface IBasketPriceClientResolver : IPriceClientResolver
    {
    }
}
