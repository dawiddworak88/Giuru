using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Buyer.Web.Shared.Definitions.Basket;
using Foundation.Pricing.DiscountCodes;
using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Services.DiscountCodes
{
    /// <summary>
    /// Only a code with proof of validation for this basket and client is exempt from fresh validation.
    /// Scoped: the basket is read at most once per request, but the binding is checked for every client passed in.
    /// </summary>
    public class BasketCookieDiscountCodeReader : IStoredDiscountCodeReader
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBasketRepository _basketRepository;
        private readonly IBasketDiscountCodeBinding _binding;
        private Task<Basket> _basket;

        public BasketCookieDiscountCodeReader(IHttpContextAccessor httpContextAccessor, IBasketRepository basketRepository, IBasketDiscountCodeBinding binding)
        {
            _httpContextAccessor = httpContextAccessor;
            _basketRepository = basketRepository;
            _binding = binding;
        }

        public async Task<string> ReadAsync(Guid? clientId, string token)
        {
            var basketCookie = _httpContextAccessor.HttpContext?.Request.Cookies[BasketConstants.BasketCookieName];

            if (!clientId.HasValue || clientId.Value == Guid.Empty || !Guid.TryParse(basketCookie, out var basketId))
            {
                return null;
            }

            var basket = await (_basket ??= _basketRepository.GetBasketById(token, CultureInfo.CurrentUICulture.Name, basketId));

            return _binding.IsBoundToClient(basketId, clientId, basket?.DiscountCode) ? basket.DiscountCode : null;
        }
    }
}
