using GraphQL.Client.Abstractions;

namespace Foundation.Content.Factories.GraphQlFactories
{
    public interface IGraphQlClientFactory
    {
        IGraphQLClient Get(string language);
    }
}
