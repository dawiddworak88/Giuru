using GraphQL.Client.Abstractions;
using GraphQL;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System;
using Foundation.Extensions.ExtensionMethods;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Identity.Api.Repositories.GraphQl
{
    public class GraphQlRepository : IGraphQlRepository
    {
        private readonly IGraphQLClient _graphQlClient;
        private readonly ILogger<GraphQlRepository> _logger;

        public GraphQlRepository(
            IGraphQLClient graphQlClient,
            ILogger<GraphQlRepository> logger)
        {
            _graphQlClient = graphQlClient;
            _logger = logger;
        }

        public async Task<string> GetTextAsync(string language, string fallbackLanguage, string attribute)
        {
            try
            {
                var response = await _graphQlClient.SendQueryAsync<JObject>(GetTextQuery(language, attribute));

                if (response.Errors.OrEmptyIfNull().Any() is false && response?.Data is not null)
                {
                    if (response.Data is null && string.IsNullOrEmpty(fallbackLanguage) is false)
                    {
                        response = await _graphQlClient.SendQueryAsync<JObject>(GetTextQuery(fallbackLanguage, attribute));
                    }

                    var data = response.Data["globalConfiguration"]?["data"]?["attributes"]?[attribute]?.ToString();

                    return data;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Couldn't get {attribute} content in language {language}");
            }

            return default;
        }

        private GraphQLRequest GetTextQuery(string language, string attribute)
        {
            return new GraphQLRequest
            {
                Query = $@"
                        query GetText {{
                          globalConfiguration(locale: ""{language}"") {{
                            data {{
                              id,
                              attributes {{
                                {attribute}
                              }}
                            }}
                          }}
                        }}"
            };
        }
    }
}
