using Buyer.Web.Shared.GraphQlResponseModels;
using Buyer.Web.Shared.DomainModels.GraphQl.Shared;
using Foundation.Extensions.ExtensionMethods;
using GraphQL;
using GraphQL.Client.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Buyer.Web.Shared.GraphQlResponseModels.NotificationBar;
using System.Collections.Generic;
using Buyer.Web.Shared.DomainModels.GraphQl.MainNavigationLinks;
using Buyer.Web.Shared.GraphQlResponseModels.MainNavigationLinks;

namespace Buyer.Web.Shared.Repositories.GraphQl
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

        public async Task<DomainModels.GraphQl.FooterLinks.Footer> GetFooterAsync(string language, string fallbackLanguage)
        {
            try
            {
                var response = await _graphQlClient.SendQueryAsync<JObject>(GetFooterContentQuery(language));

                if (response?.Data is null)
                {
                    response = await _graphQlClient.SendQueryAsync<JObject>(GetFooterContentQuery(fallbackLanguage));
                }

                var footerData = JsonConvert.DeserializeObject<FooterGraphQlResponseModel>(response.Data.ToString());

                return new DomainModels.GraphQl.FooterLinks.Footer
                {
                    Copyright = footerData?.Component?.Data?.Attributes?.Footer?.Copyright,
                    Links = footerData?.Component?.Data?.Attributes?.Footer?.Links.OrEmptyIfNull().Select(x => new Link
                    {
                        Target = x.Target,
                        Label = x.Label,
                        IsExternal = x.IsExternal,
                        Href = x.Href
                    })
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Couldn't get footer content in language ${language}");
            }

            return default;
        }

        private GraphQLRequest GetFooterContentQuery(string language)
        {
            return new GraphQLRequest
            {
                Query = @$"
                     query GetFooterContent($locale: I18NLocaleCode!){{
                      globalConfiguration(locale: $locale) {{
		                data {{
                          id,
                          attributes {{
      	                    footer {{
                              copyright,
                              links
                              {{
                                label,
                                target,
                                isExternal,
                                href
                              }}
                            }}
                          }}
	                    }}
                      }}
                    }}",
                Variables = new
                {
                    locale = language
                }
            };
        }

        public async Task<IEnumerable<DomainModels.GraphQl.NotificationBars.NotificationBarItem>> GetNotificationBar(string language, string fallbackLanguage)
        {
            try
            {
                var response = await _graphQlClient.SendQueryAsync<JObject>(GetNotificationBarQuery(language));

                if (response.Errors.OrEmptyIfNull().Any() is false && response?.Data != null)
                {
                    if (response?.Data is null && string.IsNullOrWhiteSpace(fallbackLanguage) is false)
                    {
                        response = await _graphQlClient.SendQueryAsync<JObject>(GetNotificationBarQuery(fallbackLanguage));
                    }

                    var notificationBar = JsonConvert.DeserializeObject<NotificationBarResponseModel>(response.Data.ToString());

                    return notificationBar.Component?.Data?.Attributes?.NotificationBar?.Items.OrEmptyIfNull().Select(x => new DomainModels.GraphQl.NotificationBars.NotificationBarItem
                    {
                        Icon = x.Icon,
                        Link = new Link
                        {
                            Href = x.Link.Href,
                            Label = x.Link.Label,
                            Target = x.Link.Target,
                            IsExternal = x.Link.IsExternal
                        }
                    });
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Couldn't get NotificationBar content in language ${language}");
            }

            return default;
        }

        private GraphQLRequest GetNotificationBarQuery(string language)
        {
            return new GraphQLRequest
            {
                Query = $@"
                    query GetMainNavigationLinks  {{
                      globalConfiguration(locale: ""{language}"") {{
                        data {{
                          id
                          attributes {{
                            notificationBar {{
                              notificationBarItem {{
                                icon
                                link {{
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
                    }}"
            };
        }

        public async Task<IEnumerable<MainNavigationLink>> GetMainNavigationLinksAsync(string language, string fallbackLanguage)
        {
            try
            {
                var response = await _graphQlClient.SendQueryAsync<JObject>(GetMainNavigationLinksQuery(language));

                if (response.Errors.OrEmptyIfNull().Any() is false && response?.Data != null)
                {
                    if (response.Data is null && string.IsNullOrWhiteSpace(fallbackLanguage) is false)
                    {
                        response = await _graphQlClient.SendQueryAsync<JObject>(GetMainNavigationLinksQuery(fallbackLanguage));
                    }

                    var links = JsonConvert.DeserializeObject<MainNavigationLinksGraphQlResponseModel>(response.Data.ToString());

                    return links?.Component?.Data?.Attributes?.MainNavigationLinks?.Links?.Select(x => new MainNavigationLink
                    {
                        Href = x.Href,
                        Label = x.Label,
                        Target = x.Target,
                        IsExternal = x.IsExternal
                    });
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Couldn't get MainNavigationLinks content in language ${language}");
            }

            return default;
        }

        private GraphQLRequest GetMainNavigationLinksQuery(string language)
        {
            return new GraphQLRequest
            {
                Query = $@"
                     query GetMainNavigationLinks {{
                      globalConfiguration(locale: ""{language}"") {{
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

        public async Task<string> GetTextAsync(string language, string fallbackLanguage, string attribute)
        {
            try
            {
                var response = await _graphQlClient.SendQueryAsync<JObject>(GetPersonalDataAdministratorQuer(language, attribute));

                if (response.Errors.OrEmptyIfNull().Any() is false && response?.Data is not null)
                {
                    if (response.Data is null && string.IsNullOrEmpty(fallbackLanguage) is false)
                    {
                        response = await _graphQlClient.SendQueryAsync<JObject>(GetPersonalDataAdministratorQuer(fallbackLanguage, attribute));
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

        private GraphQLRequest GetPersonalDataAdministratorQuer(string language, string attribute)
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
