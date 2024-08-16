using Catalog.Api.v1.Products.RequestModels;
using Catalog.Api.v1.Products.ResultModels;
using Foundation.ApiExtensions.Models.Response;
using Giuru.IntegrationTests.Definitions;
using System;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests
{
    [Collection(nameof(ApiCollection))]
    public class CatalogTests
    {
        private readonly ApiFixture _apiFixture;

        public CatalogTests(ApiFixture apiFixture)
        {
            _apiFixture = apiFixture;
        }

        [Fact]
        public async Task CreateUpdateGet_ReturnsProduct()
        {
            var endpointUrl = $"http://host.docker.internal:9101{ApiEndpoints.ProductsApiEndpoint}";

            var createResult = await _apiFixture.RestClient.PostAsync<ProductRequestModel, BaseResponseModel>(endpointUrl, new ProductRequestModel
            {
                Name = Products.Lamica.Name,
                Sku = Products.Lamica.Sku,
                CategoryId = Products.Lamica.CategoryId,
                IsPublished = Products.Lamica.IsPublished,
                Ean = Products.Lamica.Ean
            });

            Assert.NotNull(createResult);
            Assert.NotEqual(Guid.Empty, createResult.Data?.Id);

            var updateResult = await _apiFixture.RestClient.PostAsync<ProductRequestModel, BaseResponseModel>(endpointUrl, new ProductRequestModel
            {
                Id = createResult.Data?.Id,
                Name = Products.Lamica.UpdatedName,
                CategoryId = Products.Lamica.CategoryId,
                IsPublished = Products.Lamica.IsPublished,
                Ean = Products.Lamica.Ean
            });

            Assert.NotNull(updateResult);
            Assert.Equal(createResult.Data?.Id, updateResult.Data?.Id);

            var getResult = await _apiFixture.RestClient.GetAsync<ProductResponseModel>($"{endpointUrl}/{updateResult.Data?.Id}");

            Assert.NotNull(getResult);
            Assert.Null(getResult.Description);
            Assert.Equal(updateResult.Data?.Id, getResult.Id);
            Assert.Equal(Products.Lamica.UpdatedName, getResult.Name);
        }
    }
}
