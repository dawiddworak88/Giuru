using Foundation.ApiExtensions.Models.Response;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Paginations;
using Giuru.IntegrationTests.Definitions;
using Giuru.IntegrationTests.Helpers;
using Seller.Web.Areas.Products.ApiRequestModels;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Shared.ApiRequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests
{
    [Collection(nameof(ApiCollection))]
    public class CatalogIndexingTests
    {
        private readonly ApiFixture _apiFixture;

        public CatalogIndexingTests(ApiFixture apiFixture)
        {
            _apiFixture = apiFixture;
        }

        [Fact]
        public async Task CreateProductWithMedia_IsIndexedAndBecomesVisibleInSearch()
        {
            var images = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
            var files = new[] { Guid.NewGuid() };

            var createResult = await _apiFixture.SellerWebClient.PostAsync<SaveProductRequestModel, BaseResponseModel>(
                ApiEndpoints.ProductsApiEndpoint,
                new SaveProductRequestModel
                {
                    Name = "Indexing Media Product",
                    Sku = "IDX_MEDIA_01",
                    CategoryId = Products.Lamica.CategoryId,
                    IsPublished = true,
                    Ean = "5901234123457",
                    Images = images.Select(id => new SaveFileRequestModel { Id = id }),
                    Files = files.Select(id => new SaveFileRequestModel { Id = id })
                });

            Assert.NotNull(createResult);
            Assert.NotEqual(Guid.Empty, createResult.Id);

            var visible = await ProductWaitHelper.WaitForProductToBeVisibleAsync(_apiFixture, createResult.Id.Value);

            Assert.True(visible, $"Product {createResult.Id} did not become visible in search within the timeout");

            var getResults = await DataHelper.GetDataAsync(
                () => _apiFixture.SellerWebClient.GetAsync<PagedResults<IEnumerable<Product>>>(
                    $"{ApiEndpoints.GetProductsApiEndpoint}?pageIndex={Constants.DefaultPageIndex}&itemsPerPage={Constants.DefaultItemsPerPage}"),
                x => x != null && x.Id == createResult.Id);

            var product = getResults.Data.FirstOrDefault(x => x.Id == createResult.Id);

            Assert.NotNull(product);
            Assert.Equal("IDX_MEDIA_01", product.Sku);
            Assert.NotNull(product.Images);
            Assert.Equal(images.OrderBy(x => x), product.Images.OrderBy(x => x));
            Assert.NotNull(product.Files);
            Assert.Equal(files.OrderBy(x => x), product.Files.OrderBy(x => x));
        }
    }
}
