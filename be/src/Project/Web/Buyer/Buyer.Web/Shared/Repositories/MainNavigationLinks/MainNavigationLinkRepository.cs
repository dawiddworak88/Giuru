﻿using Buyer.Web.Shared.DomainModels.MainNavigationLinks;
using Buyer.Web.Shared.GraphQlResponseModels.MainNavigationLinks;
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

        public async Task<IEnumerable<MainNavigationLink>> GetMainNavigationLinksAsync(string contentPageKey, string language, string fallbackLanguage)
        {
            try
            { 
                var response = await _graphQlClient.SendQueryAsync<JObject>(GetMainNavigationLinksQuery(contentPageKey, language));

                if (response.Errors.OrEmptyIfNull().Any() is false && response?.Data != null)
                {
                    if(response.Data is null && string.IsNullOrWhiteSpace(fallbackLanguage) is false)
                    {
                        response = await _graphQlClient.SendQueryAsync<JObject>(GetMainNavigationLinksQuery(contentPageKey, fallbackLanguage));
                    }

                    var replacedContentPageKey = response.Data.ToString().Replace(contentPageKey, "page");

                    var links = JsonConvert.DeserializeObject<MainNavigationLinksGraphQlResponseModel>(replacedContentPageKey);

                    return links?.Page?.Data?.Attributes?.MainNavigationLinks?.Links?.Select(x => new MainNavigationLink
                    {
                        Href = x.Href,
                        Label = x.Label,
                        Taget = x.Target,
                        IsExternal = x.IsExternal
                    });
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Couldn't get links to MainNavigation");
            }

            return default;
        }

        private GraphQLRequest GetMainNavigationLinksQuery(string contentPageKey, string language)
        {
            return new GraphQLRequest
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
                    }}"
            };
        }
    }
}