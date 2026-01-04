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
using Giuru.IntegrationTests.Helpers;

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
            var newProductId = await InventoryHelper.CreateProductAndAddToStockAsync(_apiFixture, new ProductRequestModel
            {
                Name = Products.Lamica.Name,
                Sku = Products.Lamica.Sku,
                CategoryId = Products.Lamica.CategoryId,
                IsPublished = Products.Lamica.IsPublished,
                Ean = Products.Lamica.Ean
            });

            var updateProductId = await InventoryHelper.CreateProductAndAddToStockAsync(_apiFixture, new ProductRequestModel
            {
                Id = newProductId,
                Name = Products.Lamica.UpdatedName,
                Sku = Products.Lamica.Sku,
                CategoryId = Products.Lamica.CategoryId,
                IsPublished = Products.Lamica.IsPublished,
                Ean = Products.Lamica.Ean
            });

            Assert.Equal(newProductId, updateProductId);

            var getResults = await DataHelper.GetDataAsync(
                () => _apiFixture.SellerWebClient.GetAsync<PagedResults<IEnumerable<Product>>>($"{ApiEndpoints.GetProductsApiEndpoint}?pageIndex={Constants.DefaultPageIndex}&itemsPerPage={Constants.DefaultItemsPerPage}"),
                x => x != null && x.Id == updateProductId);

            Assert.NotNull(getResults);
            Assert.NotNull(getResults.Data);
            var product = getResults.Data.FirstOrDefault(x => x.Id == updateProductId);
            Assert.NotNull(product);
            Assert.Null(product.Description);
        }
    }
}
