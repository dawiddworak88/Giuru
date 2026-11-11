using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.Extensions.Exceptions;
using Foundation.Pricing.Services;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Net;
using System.Threading.Tasks;
using BuyerRepository = Buyer.Web.Shared.Repositories.Clients.DiscountCodeEligibilityRepository;
using BuyerRequest = Buyer.Web.Shared.ApiRequestModels.Clients.DiscountCodeEligibilityRequestModel;
using BuyerSettings = Buyer.Web.Shared.Configurations.AppSettings;
using BuyerValidCode = Buyer.Web.Shared.DomainModels.Clients.ValidDiscountCode;
using SellerRepository = Seller.Web.Areas.Clients.Repositories.DiscountCodes.DiscountCodeEligibilityRepository;
using SellerRequest = Seller.Web.Areas.Clients.ApiRequestModels.DiscountCodeEligibilityRequestModel;
using SellerSettings = Seller.Web.Shared.Configurations.AppSettings;
using SellerValidCode = Seller.Web.Areas.Clients.DomainModels.ValidDiscountCode;

namespace Giuru.UnitTests.Clients.DiscountCodes
{
    // Client.Api answers "not valid" with no content, so a not found can only mean a missing route
    // and must fail the save instead of silently dropping a code that may still be valid.
    public class DiscountCodeEligibilityRepositoryTests
    {
        public enum Portal { Buyer, Seller }

        [Theory]
        [InlineData(Portal.Buyer)]
        [InlineData(Portal.Seller)]
        public async Task WhenClientApiAnswersWithACode_ReturnsItsStoredSpelling(Portal portal)
        {
            var repository = CreateRepository(portal, HttpStatusCode.OK, "WINTER25");

            Assert.Equal("WINTER25", await repository.GetValidDiscountCodeAsync("token", "en", Guid.NewGuid(), "winter25"));
        }

        [Theory]
        [InlineData(Portal.Buyer)]
        [InlineData(Portal.Seller)]
        public async Task WhenClientApiAnswersWithNoContent_ReturnsNull(Portal portal)
        {
            var repository = CreateRepository(portal, HttpStatusCode.NoContent, code: null);

            Assert.Null(await repository.GetValidDiscountCodeAsync("token", "en", Guid.NewGuid(), "WINTER25"));
        }

        [Theory]
        [InlineData(Portal.Buyer, HttpStatusCode.NotFound)]
        [InlineData(Portal.Seller, HttpStatusCode.NotFound)]
        [InlineData(Portal.Buyer, HttpStatusCode.ServiceUnavailable)]
        [InlineData(Portal.Seller, HttpStatusCode.ServiceUnavailable)]
        public async Task WhenClientApiFails_Throws(Portal portal, HttpStatusCode statusCode)
        {
            var repository = CreateRepository(portal, statusCode, code: null);

            await Assert.ThrowsAsync<CustomException>(() => repository.GetValidDiscountCodeAsync("token", "en", Guid.NewGuid(), "WINTER25"));
        }

        private static IDiscountCodeEligibilityRepository CreateRepository(Portal portal, HttpStatusCode statusCode, string code)
        {
            var apiClientService = Substitute.For<IApiClientService>();
            var isSuccessStatusCode = (int)statusCode is >= 200 and < 300;

            if (portal == Portal.Buyer)
            {
                apiClientService.GetAsync<ApiRequest<BuyerRequest>, BuyerRequest, BuyerValidCode>(Arg.Any<ApiRequest<BuyerRequest>>())
                    .Returns(new ApiResponse<BuyerValidCode>
                    {
                        StatusCode = statusCode,
                        IsSuccessStatusCode = isSuccessStatusCode,
                        Data = code is null ? null : new BuyerValidCode { Code = code }
                    });

                return new BuyerRepository(apiClientService, Options.Create(new BuyerSettings { ClientUrl = "http://client" }));
            }

            apiClientService.GetAsync<ApiRequest<SellerRequest>, SellerRequest, SellerValidCode>(Arg.Any<ApiRequest<SellerRequest>>())
                .Returns(new ApiResponse<SellerValidCode>
                {
                    StatusCode = statusCode,
                    IsSuccessStatusCode = isSuccessStatusCode,
                    Data = code is null ? null : new SellerValidCode { Code = code }
                });

            return new SellerRepository(apiClientService, Options.Create(new SellerSettings { ClientUrl = "http://client" }));
        }
    }
}
