using Catalog.Api.v1.Products.RequestModels;
using Client.Api.v1.RequestModels;
using Client.Api.v1.ResponseModels;
using Foundation.ApiExtensions.Models.Response;
using Foundation.GenericRepository.Paginations;
using Giuru.IntegrationTests.Definitions;
using Seller.Web.Areas.Products.ApiResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task CreateUpdateGet_ReturnsClient()
        {
            var createResult = await _apiFixture.RestClient.PostAsync<ClientRequestModel, BaseResponseModel>(ApiEndpoints.ClientsApiEndpoint, new ClientRequestModel
            {
                Name = Clients.Name,
                Email = Clients.Email,
                CommunicationLanguage = Clients.Language,
                OrganisationId = Clients.OrganisationId
            });

            Assert.NotNull(createResult);
            Assert.NotEqual(Guid.Empty, createResult.Data?.Id);

            var updateResult = await _apiFixture.RestClient.PostAsync<ClientRequestModel, BaseResponseModel>(ApiEndpoints.ClientsApiEndpoint, new ClientRequestModel
            {
                Id = createResult.Data?.Id,
                Name = Clients.UpdatedName,
                Email = Clients.Email,
                CommunicationLanguage = Clients.Language,
                OrganisationId = Clients.OrganisationId
            });

            Assert.NotNull(updateResult);
            Assert.Equal(createResult.Data?.Id, updateResult.Data?.Id);

            var getResult = await _apiFixture.RestClient.GetAsync<PagedResults<IEnumerable<ClientResponseModel>>>($"{ApiEndpoints.GetClientsApiEndpoint}?pageIndex=1&itemsPerPage=25");

            Assert.NotNull(getResult.Data);
            Assert.Equal(1, getResult.Total);
            Assert.Equal(updateResult.Data?.Id, getResult.Data.FirstOrDefault().Id);
            Assert.Equal(Clients.UpdatedName, getResult.Data.FirstOrDefault().Name);
        }

        [Fact]
        public async Task CreateUpdateGet_ReturnsProduct()
        {
            var createResult = await _apiFixture.RestClient.PostAsync<ProductRequestModel, ProductResponseModel>(ApiEndpoints.ProductsApiEndpoint, new ProductRequestModel
            {
                Name = Products.Lamica.Name,
                Sku = Products.Lamica.Sku,
                CategoryId = Products.Lamica.CategoryId,
                IsPublished = Products.Lamica.IsPublished,
                Ean = Products.Lamica.Ean
            });

            Assert.NotNull(createResult);
            Assert.NotEqual(Guid.Empty, createResult.Data?.Id);

            var updateResult = await _apiFixture.RestClient.PostAsync<ProductRequestModel, BaseResponseModel>(ApiEndpoints.ProductsApiEndpoint, new ProductRequestModel
            {
                Id = createResult.Data?.Id,
                Name = Products.Lamica.UpdatedName,
                CategoryId = Products.Lamica.CategoryId,
                IsPublished = Products.Lamica.IsPublished,
                Ean = Products.Lamica.Ean
            });

            Assert.NotNull(updateResult);
            Assert.Equal(createResult.Data?.Id, updateResult.Data?.Id);


            var getResult = await _apiFixture.RestClient.GetAsync<PagedResults<IEnumerable<ProductResponseModel>>>($"{ApiEndpoints.GetProductsApiEndpoint}?pageIndex=1&itemsPerPage=25");

            Assert.NotNull(getResult);
            /*Assert.Null(getResult.Data.FirstOrDefault().des);
            Assert.Equal(updateResult.Data?.Id, getResult.Id);
            Assert.Equal(Products.Lamica.UpdatedName, getResult.Name);*/
        }
    }
}
