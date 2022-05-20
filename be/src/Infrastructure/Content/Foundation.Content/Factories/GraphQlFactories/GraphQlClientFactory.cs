using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

namespace Foundation.Content.Factories.GraphQlFactories
{
    public class GraphQlClientFactory : IGraphQlClientFactory
    {
        private readonly string graphQlUrl;
        private readonly string authorizationKey;

        public GraphQlClientFactory(string graphQlUrl, string authorizationKey)
        {
            this.graphQlUrl = graphQlUrl;
            this.authorizationKey = authorizationKey;
        }

        public IGraphQLClient Get(string language)
        {
            var graphQlClient = new GraphQLHttpClient(this.graphQlUrl, new NewtonsoftJsonSerializer());
            graphQlClient.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.authorizationKey}");
            graphQlClient.HttpClient.DefaultRequestHeaders.Add("Accept-Language", language);

            return graphQlClient;
        }
    }
}
