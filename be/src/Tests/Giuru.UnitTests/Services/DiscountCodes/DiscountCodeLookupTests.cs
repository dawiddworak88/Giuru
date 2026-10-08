using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.Extensions.Exceptions;
using Foundation.Pricing.DiscountCodes;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using BuyerAppSettings = Buyer.Web.Shared.Configurations.AppSettings;
using BuyerDiscountCodesRepository = Buyer.Web.Shared.Repositories.Clients.DiscountCodesRepository;
using BuyerLookup = Buyer.Web.Shared.Services.DiscountCodes.ClientApiDiscountCodeLookup;
using BuyerRequest = Buyer.Web.Shared.ApiRequestModels.DiscountCodeValidationApiRequestModel;
using BuyerResponse = Buyer.Web.Shared.ApiResponseModels.DiscountCodeValidationApiResponseModel;
using SellerAppSettings = Seller.Web.Shared.Configurations.AppSettings;
using SellerDiscountCodesRepository = Seller.Web.Areas.Clients.Repositories.DiscountCodes.DiscountCodesRepository;
using SellerLookup = Seller.Web.Shared.Services.DiscountCodes.ClientApiDiscountCodeLookup;
using SellerRequest = Seller.Web.Areas.Clients.ApiRequestModels.DiscountCodeValidationApiRequestModel;
using SellerResponse = Seller.Web.Areas.Clients.ApiResponseModels.DiscountCodeValidationApiResponseModel;

namespace Giuru.UnitTests.Services.DiscountCodes
{
    // The HTTP side of the verification: one call to Client.Api, and a failed call is never read as an answer.
    public class DiscountCodeLookupTests
    {
        private const string ClientUrl = "http://client-api";
        private static readonly Guid ClientId = Guid.NewGuid();

        private static ApiResponse<T> Response<T>(T data, HttpStatusCode status = HttpStatusCode.OK, string message = null) where T : class
        {
            return new ApiResponse<T>
            {
                Data = data,
                StatusCode = status,
                IsSuccessStatusCode = (int)status is >= 200 and < 300,
                Message = message
            };
        }

        public class SellerSide
        {
            private static (SellerDiscountCodesRepository Repository, IApiClientService Api) Create(ApiResponse<SellerResponse> response)
            {
                var api = Substitute.For<IApiClientService>();
                api.GetAsync<ApiRequest<SellerRequest>, SellerRequest, SellerResponse>(Arg.Any<ApiRequest<SellerRequest>>())
                    .Returns(Task.FromResult(response));

                var settings = Options.Create(new SellerAppSettings { ClientUrl = ClientUrl });

                return (new SellerDiscountCodesRepository(api, settings), api);
            }

            [Fact]
            public async Task ValidateAsync_SendsTheTokenTheClientAndTheRawCodeToTheValidationEndpoint()
            {
                var (repository, api) = Create(Response(new SellerResponse { Status = "Valid", DiscountCode = "SUMMER25" }));
                var code = "SUMMER 25&x=1/é";

                var validation = await repository.ValidateAsync("token", "en", ClientId, code);

                Assert.True(validation.IsValid);
                Assert.Equal("SUMMER25", validation.DiscountCode);

                // The API client service URL-encodes the query values itself, so the repository must hand over the raw text.
                await api.Received(1).GetAsync<ApiRequest<SellerRequest>, SellerRequest, SellerResponse>(
                    Arg.Is<ApiRequest<SellerRequest>>(x =>
                        x.AccessToken == "token"
                        && x.Data.ClientId == ClientId
                        && x.Data.Code == code
                        && x.EndpointAddress.StartsWith(ClientUrl)
                        && x.EndpointAddress.EndsWith("validation")));
            }

            [Theory]
            [InlineData("NotFound", DiscountCodeValidationStatus.NotFound)]
            [InlineData("Disabled", DiscountCodeValidationStatus.Disabled)]
            [InlineData("NotAssigned", DiscountCodeValidationStatus.NotAssigned)]
            [InlineData("ClientUnknown", DiscountCodeValidationStatus.ClientUnknown)]
            [InlineData("NotApplicable", DiscountCodeValidationStatus.NotApplicable)]
            public async Task ValidateAsync_MapsAnInapplicableStatusWithoutACode(string status, DiscountCodeValidationStatus expected)
            {
                var (repository, _) = Create(Response(new SellerResponse { Status = status, DiscountCode = "SUMMER25" }));

                var validation = await repository.ValidateAsync("token", "en", ClientId, "SUMMER25");

                Assert.Equal(expected, validation.Status);
                Assert.Null(validation.DiscountCode);
            }

            [Theory]
            [InlineData(HttpStatusCode.InternalServerError)]
            [InlineData(HttpStatusCode.Unauthorized)]
            [InlineData(HttpStatusCode.NotFound)]
            [InlineData(HttpStatusCode.ServiceUnavailable)]
            public async Task ValidateAsync_WhenTheCallFails_Throws(HttpStatusCode status)
            {
                var (repository, _) = Create(Response<SellerResponse>(null, status, "boom"));

                var exception = await Assert.ThrowsAsync<CustomException>(() => repository.ValidateAsync("token", "en", ClientId, "SUMMER25"));

                Assert.Equal((int)status, exception.StatusCode);
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("Banana")]
            public async Task ValidateAsync_WhenTheStatusIsMissingOrUnknown_IsUnavailable(string status)
            {
                var (repository, _) = Create(Response(new SellerResponse { Status = status, DiscountCode = "SUMMER25" }));

                var validation = await repository.ValidateAsync("token", "en", ClientId, "SUMMER25");

                Assert.Equal(DiscountCodeValidationStatus.Unavailable, validation.Status);
            }

            [Fact]
            public async Task ValidateAsync_WhenTheBodyIsEmpty_IsUnavailable()
            {
                var (repository, _) = Create(Response<SellerResponse>(null));

                var validation = await repository.ValidateAsync("token", "en", ClientId, "SUMMER25");

                Assert.Equal(DiscountCodeValidationStatus.Unavailable, validation.Status);
            }

            [Fact]
            public async Task ValidateAsync_WhenAlreadyCancelled_DoesNotCallClientApi()
            {
                var (repository, api) = Create(Response(new SellerResponse { Status = "Valid", DiscountCode = "SUMMER25" }));
                using var cancellation = new CancellationTokenSource();
                cancellation.Cancel();

                await Assert.ThrowsAnyAsync<OperationCanceledException>(
                    () => repository.ValidateAsync("token", "en", ClientId, "SUMMER25", cancellation.Token));

                await api.DidNotReceiveWithAnyArgs().GetAsync<ApiRequest<SellerRequest>, SellerRequest, SellerResponse>(default);
            }

            [Fact]
            public async Task TheLookup_DelegatesToTheRepositoryWithTheExplicitClient()
            {
                var (repository, api) = Create(Response(new SellerResponse { Status = "Valid", DiscountCode = "SUMMER25" }));

                var validation = await new SellerLookup(repository).ValidateAsync(ClientId, "summer25", "token");

                Assert.Equal("SUMMER25", validation.DiscountCode);
                await api.Received(1).GetAsync<ApiRequest<SellerRequest>, SellerRequest, SellerResponse>(
                    Arg.Is<ApiRequest<SellerRequest>>(x => x.Data.ClientId == ClientId && x.Data.Code == "summer25"));
            }

            [Fact]
            public async Task TheValidator_TurnsAFailedCallIntoUnavailableInsteadOfAnAnswer()
            {
                var (repository, _) = Create(Response<SellerResponse>(null, HttpStatusCode.InternalServerError, "boom"));
                var validator = new DiscountCodeValidator(new SellerLookup(repository), Substitute.For<Microsoft.Extensions.Logging.ILogger<DiscountCodeValidator>>());

                var validation = await validator.ValidateAsync(ClientId, "SUMMER25", "token");

                Assert.Equal(DiscountCodeValidationStatus.Unavailable, validation.Status);
            }
        }

        public class BuyerSide
        {
            private static (BuyerDiscountCodesRepository Repository, IApiClientService Api) Create(ApiResponse<BuyerResponse> response)
            {
                var api = Substitute.For<IApiClientService>();
                api.GetAsync<ApiRequest<BuyerRequest>, BuyerRequest, BuyerResponse>(Arg.Any<ApiRequest<BuyerRequest>>())
                    .Returns(Task.FromResult(response));

                var settings = Options.Create(new BuyerAppSettings { ClientUrl = ClientUrl });

                return (new BuyerDiscountCodesRepository(api, settings), api);
            }

            [Fact]
            public async Task ValidateAsync_SendsTheTokenTheClientAndTheRawCodeToTheValidationEndpoint()
            {
                var (repository, api) = Create(Response(new BuyerResponse { Status = "Valid", DiscountCode = "SUMMER25" }));
                var code = "SUMMER 25&x=1/é";

                var validation = await repository.ValidateAsync("token", "en", ClientId, code);

                Assert.True(validation.IsValid);
                await api.Received(1).GetAsync<ApiRequest<BuyerRequest>, BuyerRequest, BuyerResponse>(
                    Arg.Is<ApiRequest<BuyerRequest>>(x =>
                        x.AccessToken == "token"
                        && x.Data.ClientId == ClientId
                        && x.Data.Code == code
                        && x.EndpointAddress.StartsWith(ClientUrl)
                        && x.EndpointAddress.EndsWith("validation")));
            }

            [Fact]
            public async Task ValidateAsync_ForABuyer_NeverSurfacesACodeWithAnInapplicableStatus()
            {
                var (repository, _) = Create(Response(new BuyerResponse { Status = "NotApplicable", DiscountCode = "SUMMER25" }));

                var validation = await repository.ValidateAsync("token", "en", ClientId, "SUMMER25");

                Assert.Equal(DiscountCodeValidationStatus.NotApplicable, validation.Status);
                Assert.Null(validation.DiscountCode);
            }

            [Theory]
            [InlineData(HttpStatusCode.InternalServerError)]
            [InlineData(HttpStatusCode.Unauthorized)]
            [InlineData(HttpStatusCode.ServiceUnavailable)]
            public async Task ValidateAsync_WhenTheCallFails_Throws(HttpStatusCode status)
            {
                var (repository, _) = Create(Response<BuyerResponse>(null, status, "boom"));

                await Assert.ThrowsAsync<CustomException>(() => repository.ValidateAsync("token", "en", ClientId, "SUMMER25"));
            }

            [Theory]
            [InlineData(null)]
            [InlineData("Banana")]
            public async Task ValidateAsync_WhenTheStatusIsMissingOrUnknown_IsUnavailable(string status)
            {
                var (repository, _) = Create(Response(new BuyerResponse { Status = status, DiscountCode = "SUMMER25" }));

                var validation = await repository.ValidateAsync("token", "en", ClientId, "SUMMER25");

                Assert.Equal(DiscountCodeValidationStatus.Unavailable, validation.Status);
            }

            [Fact]
            public async Task ValidateAsync_WhenTheBodyIsEmpty_IsUnavailable()
            {
                var (repository, _) = Create(Response<BuyerResponse>(null));

                var validation = await repository.ValidateAsync("token", "en", ClientId, "SUMMER25");

                Assert.Equal(DiscountCodeValidationStatus.Unavailable, validation.Status);
            }

            [Fact]
            public async Task TheLookup_DelegatesToTheRepositoryWithTheExplicitClient()
            {
                var (repository, api) = Create(Response(new BuyerResponse { Status = "Valid", DiscountCode = "SUMMER25" }));

                var validation = await new BuyerLookup(repository).ValidateAsync(ClientId, "summer25", "token");

                Assert.Equal("SUMMER25", validation.DiscountCode);
                await api.Received(1).GetAsync<ApiRequest<BuyerRequest>, BuyerRequest, BuyerResponse>(
                    Arg.Is<ApiRequest<BuyerRequest>>(x => x.Data.ClientId == ClientId && x.Data.Code == "summer25"));
            }
        }
    }
}
