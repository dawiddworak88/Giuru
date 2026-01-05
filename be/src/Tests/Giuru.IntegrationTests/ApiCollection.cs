namespace Giuru.IntegrationTests
{
    [CollectionDefinition(nameof(ApiCollection), DisableParallelization = true)]
    public class ApiCollection : ICollectionFixture<ApiFixture>
    {
    }
}
