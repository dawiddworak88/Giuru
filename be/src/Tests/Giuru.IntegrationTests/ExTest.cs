using System.Threading.Tasks;

namespace Giuru.IntegrationTests
{
    [Collection(nameof(ApiCollection))]
    public class ExTest
    {
        [Fact]
        public async Task Test()
        {
            Assert.True(true);
        }
    }
}
