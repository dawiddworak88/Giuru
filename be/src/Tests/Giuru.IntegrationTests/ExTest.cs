using System.Threading.Tasks;

namespace Giuru.IntegrationTests
{
    [Collection(nameof(ApiCollection))]
    public class ExTest
    {
        private readonly ApiFixture _apiFixture;

        public ExTest(ApiFixture apiFixture)
        {
            _apiFixture = apiFixture;
        }

        [Fact]
        public async Task Test()
        {
            Assert.True(true);
        }
    }
}
