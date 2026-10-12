namespace Foundation.Pricing.Services
{
    /// <summary>
    /// The resolver the basket save and upload paths use to reprice. It keeps the discount code the basket
    /// coordinator hands it and never revalidates it: a code that is already stored on a basket stays in
    /// force until the user replaces or removes it, even if it has since been revoked.
    /// Fresh product price reads use <see cref="IPriceClientResolver"/>, which is validating.
    /// </summary>
    public interface IBasketPriceClientResolver : IPriceClientResolver
    {
    }
}
