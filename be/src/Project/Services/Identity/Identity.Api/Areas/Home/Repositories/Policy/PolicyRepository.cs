using GraphQL.Client.Abstractions;
using GraphQL;
using Identity.Api.Areas.Home.ResponseModels.Policy;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using Identity.Api.Areas.Home.Repositories.Content;
using Microsoft.Extensions.Logging;
using System.Linq;
using Foundation.Extensions.ExtensionMethods;

namespace Identity.Api.Areas.Home.Repositories.Policy
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly IGraphQLClient _graphQlClient;
        private readonly ILogger<ContentRepository> _logger;

        public PolicyRepository(
            IGraphQLClient graphQLClient,
            ILogger<ContentRepository> logger)
        {
            _graphQlClient = graphQLClient;
            _logger = logger;
        }

        public async Task<DomainModels.Policy> GetPolicyAsync(string language)
        {
            try
            {
                var query = new GraphQLRequest
                {
                    Query = $@"query getPrivacyPolicy {{
                      privacyPolicyPage(locale: ""{language}"") {{
                        data {{
                          id
                          attributes {{
                            policy {{
                              title
                              description
                              accordion {{
                                accordionItems {{
                                  id
                                  title
                                  description
                                }}
                              }}
                            }}
                          }}
                        }}
                      }}
                    }}"
                };

                var response = await _graphQlClient.SendQueryAsync<JObject>(query);

                if (response.Errors.OrEmptyIfNull().Any() is false && response?.Data is not null)
                {
                    var metaData = JsonConvert.DeserializeObject<PolicyGraphQlResponseModel>(response.Data.ToString());

                    return new DomainModels.Policy
                    {
                        Title = metaData.Page?.Data?.Attributes?.Policy?.Title,
                        Description = metaData.Page?.Data?.Attributes?.Policy?.Description,
                        Accordions = metaData.Page?.Data?.Attributes?.Policy?.Accordion?.AccordionItems.Select(x => new DomainModels.AccordionItem
                        {
                            Title = x.Title,
                            Description = x.Description
                        })
                    };
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Couldn't get policy for privacy policy in language ${language}");
            }

            return default;
        }
    }
}
