using Catalog.Api.v1.Products.RequestModels;
using Client.Api.v1.RequestModels;
using Client.Api.v1.ResponseModels;
using Foundation.ApiExtensions.Models.Response;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Paginations;
using Giuru.IntegrationTests.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.DomainModels;

namespace Giuru.IntegrationTests
{
    [Collection(nameof(ApiCollection))]
    public class SellerWebTests
    {
        private readonly ApiFixture _apiFixture;

        public SellerWebTests(ApiFixture apiFixture)
        {
            _apiFixture = apiFixture;
        }

        [Fact]
        public async Task CreateUpdateClientAndGet_ReturnsClient()
        {
            var createResult = await _apiFixture.SellerWebClient.PostAsync<ClientRequestModel, BaseResponseModel>(ApiEndpoints.ClientsApiEndpoint, new ClientRequestModel
            {
                Name = Clients.Name,
                Email = Clients.Email,
                CommunicationLanguage = Clients.Language,
                OrganisationId = Clients.OrganisationId
            });

            Assert.NotNull(createResult);
            Assert.NotEqual(Guid.Empty, createResult.Id);

            var updatedResult = await _apiFixture.SellerWebClient.PostAsync<ClientRequestModel, BaseResponseModel>(ApiEndpoints.ClientsApiEndpoint, new ClientRequestModel
            {
                Id = createResult.Id,
                Name = Clients.UpdatedName,
                Email = Clients.Email,
                CommunicationLanguage = Clients.Language,
                OrganisationId = Clients.OrganisationId
            });

            Assert.NotNull(updatedResult);
            Assert.Equal(createResult.Id, updatedResult.Id);

            var getResults = await _apiFixture.SellerWebClient.GetAsync<PagedResults<IEnumerable<ClientResponseModel>>>($"{ApiEndpoints.GetClientsApiEndpoint}?pageIndex={Constants.DefaultPageIndex}&itemsPerPage={Constants.DefaultItemsPerPage}");

            Assert.NotNull(getResults.Data);
            Assert.Equal(1, getResults.Total);
            Assert.Equal(updatedResult.Id, getResults.Data.FirstOrDefault().Id);
            Assert.Equal(Clients.UpdatedName, getResults.Data.FirstOrDefault().Name);
        }

        [Fact]
        public async Task CreateUpdateProductAndGet_ReturnsProduct()
        {
            var createResult = await _apiFixture.SellerWebClient.PostAsync<ProductRequestModel, BaseResponseModel>(ApiEndpoints.ProductsApiEndpoint, new ProductRequestModel
            {
                Name = Products.Lamica.Name,
                Sku = Products.Lamica.Sku,
                CategoryId = Products.Lamica.CategoryId,
                IsPublished = Products.Lamica.IsPublished,
                Ean = Products.Lamica.Ean
            });

            Assert.NotNull(createResult);
            Assert.NotEqual(Guid.Empty, createResult.Id);

            var updatedResult = await _apiFixture.SellerWebClient.PostAsync<ProductRequestModel, BaseResponseModel>(ApiEndpoints.ProductsApiEndpoint, new ProductRequestModel
            {
                Id = createResult.Id,
                Name = Products.Lamica.UpdatedName,
                Sku = Products.Lamica.Sku,
                CategoryId = Products.Lamica.CategoryId,
                IsPublished = Products.Lamica.IsPublished,
                Ean = Products.Lamica.Ean
            });

            Assert.NotNull(updatedResult);
            Assert.Equal(createResult.Id, updatedResult.Id);

            int timeoutInSeconds = 30;
            int elapsedSeconds = 0;
            PagedResults<IEnumerable<Product>> getResults = null;

            while (elapsedSeconds < timeoutInSeconds)
            {
                getResults = await _apiFixture.SellerWebClient.GetAsync<PagedResults<IEnumerable<Product>>>($"{ApiEndpoints.GetProductsApiEndpoint}?pageIndex={Constants.DefaultPageIndex}&itemsPerPage={Constants.DefaultItemsPerPage}");

                if (getResults.Data.FirstOrDefault() != null)
                {
                    break;
                }

                await Task.Delay(1000);
                elapsedSeconds++;
            }

            Assert.NotNull(getResults);
            Assert.Null(getResults.Data.FirstOrDefault().Description);
            Assert.Equal(updatedResult.Id, getResults.Data.FirstOrDefault().Id);
        }
    }
}
