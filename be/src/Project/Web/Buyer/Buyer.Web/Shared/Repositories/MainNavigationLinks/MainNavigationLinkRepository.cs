using Buyer.Web.Shared.DomainModels.MainNavigationLinks;
using Buyer.Web.Shared.GraphQlResponseModels;
using Foundation.Extensions.ExtensionMethods;
using GraphQL;
using GraphQL.Client.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.MainNavigationLinks
{
    public class MainNavigationLinkRepository : IMainNavigationLinkRepository
    {
        private readonly IGraphQLClient _graphQlClient;
        private readonly ILogger<MainNavigationLinkRepository> _logger;

        public MainNavigationLinkRepository(
            IGraphQLClient graphQlClient,
            ILogger<MainNavigationLinkRepository> logger)
        {
            _graphQlClient = graphQlClient;
            _logger = logger;
        }

        public async Task<ICollection<MainNavigationLink>> GetMainNavigationLinksAsync(string contentPageKey, string language)
        {
            try 
            {
                var query = new GraphQLRequest
                {
                    Query = $@"
                     query GetMainNavigationLinks {{
                      {contentPageKey}(locale: ""{language}"") {{
                        data {{
                          id,
                          attributes {{
                            mainNavigationLinks {{
                              links {{
                                href
                                label
                                target
                                isExternal
                              }}
                            }}
                          }}
                        }}
                      }}
                    }}
                    "
                };

                var response = await _graphQlClient.SendQueryAsync<JObject>(query);

                if( response.Errors.OrEmptyIfNull().Any() is false && response?.Data != null)
                {
                    var replacedContentPageKey = response.Data.ToString().Replace(contentPageKey, "page");

                    var metaData = JsonConvert.DeserializeObject<ICollection<MainNavigationLink>>(replacedContentPageKey);

                    return metaData;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Couldn't get link to MainNavigation");
            }

            return default;
        }
    }
}
