using System;
using System.Threading.Tasks;

namespace Giuru.IntegrationTests
{
    [Collection(nameof(ApiCollection))]
    public class CatalogApiController
    {
        private readonly ApiFixture _apiFixture;

        public CatalogApiController(ApiFixture apiFixture)
        {
            _apiFixture = apiFixture;
        }

        [Fact]
        public async Task Get_Client()
        {

        }

        [Fact]
        public async Task CreateUpdateProduct()
        {
            //var response = await _apiFixture.RestClient.GetAsync<Test, Test>($"http://host.docker.internal:9101/api/v1/products", new Test { Id = Guid.NewGuid() });
        }

        public class Test
        {
            public Guid Id { get; set; }
        }
    }
}
