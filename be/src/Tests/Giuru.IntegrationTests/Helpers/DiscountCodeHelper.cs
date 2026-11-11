using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Shared.Definitions;
using Giuru.IntegrationTests.Definitions;
using System;
using System.Linq;
using System.Threading.Tasks;
using DiscountCodeRequestModel = Seller.Web.Areas.Clients.ApiRequestModels.DiscountCodeRequestModel;
using SellerBasketApiResponseModel = Seller.Web.Areas.Orders.ApiResponseModels.BasketApiResponseModel;
using SellerBasketItemRequestModel = Seller.Web.Areas.Orders.ApiRequestModels.BasketItemRequestModel;
using SellerBasketResponseModel = Seller.Web.Areas.Orders.ApiResponseModels.BasketResponseModel;
using SellerSaveBasketRequestModel = Seller.Web.Areas.Orders.ApiRequestModels.SaveBasketRequestModel;

namespace Giuru.IntegrationTests.Helpers
{
    /// <summary>
    /// Discount Codes and Baskets for the discount-code tests. Basket lines use a SKU no catalog product
    /// has, so a save runs the real eligibility check and price-client lookup but has nothing to price.
    /// </summary>
    public static class DiscountCodeHelper
    {
        public static string NewCode()
        {
            return $"T{Guid.NewGuid():N}"[..16].ToUpperInvariant();
        }

        public static string NewUnknownSku()
        {
            return $"NO-PRODUCT-{Guid.NewGuid():N}";
        }

        public static async Task<Guid> CreateCodeAsync(ApiFixture apiFixture, string code, params Guid[] clientIds)
        {
            var created = await SaveCodeAsync(apiFixture, new DiscountCodeRequestModel { Code = code, ClientIds = clientIds });

            return created.Id.Value;
        }

        public static Task<BaseResponseModel> SaveCodeAsync(ApiFixture apiFixture, DiscountCodeRequestModel request)
        {
            return apiFixture.SellerWebClient.PostAsync<DiscountCodeRequestModel, BaseResponseModel>(ApiEndpoints.DiscountCodesApiEndpoint, request);
        }

        public static Task<SellerBasketResponseModel> SaveSellerBasketAsync(ApiFixture apiFixture, Guid basketId, Guid clientId, string discountCode)
        {
            return apiFixture.SellerWebClient.PostAsync<SellerSaveBasketRequestModel, SellerBasketResponseModel>(
                ApiEndpoints.BasketApiEndpoint,
                CreateSellerBasketRequest(basketId, clientId, discountCode));
        }

        public static SellerSaveBasketRequestModel CreateSellerBasketRequest(Guid basketId, Guid clientId, string discountCode, int lineCount = 1)
        {
            return new SellerSaveBasketRequestModel
            {
                Id = basketId,
                ClientId = clientId,
                DiscountCode = discountCode,
                Items = Enumerable.Range(0, lineCount)
                    .Select(_ => new SellerBasketItemRequestModel
                    {
                        ProductId = Guid.NewGuid(),
                        Sku = NewUnknownSku(),
                        Name = "Unpriced line",
                        Quantity = 1
                    })
                    .ToList()
            };
        }

        public static Task<SellerBasketApiResponseModel> GetPersistedBasketAsync(ApiFixture apiFixture, Guid basketId)
        {
            return apiFixture.BasketApiClient.GetAsync<SellerBasketApiResponseModel>($"{ApiConstants.Baskets.BasketsApiEndpoint}/{basketId}");
        }
    }
}
