using Catalog.Api.v1.Products.RequestModels;
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
                CategoryId = Products.Lamica.CategoryId,
                IsPublished = Products.Lamica.IsPublished,
                Ean = Products.Lamica.Ean
            });

            Assert.NotNull(createResult);
            Assert.NotEqual(Guid.Empty, createResult.Data?.Id);
        }
    }
}
