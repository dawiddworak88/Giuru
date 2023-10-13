using Buyer.Web.Shared.DomainModels.GraphQl;
using Buyer.Web.Shared.GraphQlResponseModels;
using Foundation.Extensions.ExtensionMethods;
using GraphQL;
using GraphQL.Client.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<Shared.DomainModels.GraphQl.Footer> GetFooterAsync(string contentPageKey, string language, string fallbackLanguage)
        {
            try
            {
                var response = await _graphQlClient.SendQueryAsync<JObject>(GetFooterContentQuery(contentPageKey, language));

                if (response?.Data is null)
                {
                    response = await _graphQlClient.SendQueryAsync<JObject>(GetFooterContentQuery(contentPageKey, fallbackLanguage));
                }

                var replacedContentPageKey = response.Data.ToString().Replace(contentPageKey, "page");

                var footerData = JsonConvert.DeserializeObject<FooterGraphQlResponseModel>(replacedContentPageKey);

                return new Shared.DomainModels.GraphQl.Footer
                {
                    Copyright = footerData?.Page?.Data?.Attributes?.Footer?.Copyright,
                    Links = footerData?.Page?.Data?.Attributes?.Footer?.Links.OrEmptyIfNull().Select(x => new Link
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
                _logger.LogError(exception, $"Couldn't get footer content for ${contentPageKey} in language ${language}");
            }

            return default;
        }

        private GraphQLRequest GetFooterContentQuery(string contentPageKey, string language)
        {
            return new GraphQLRequest
            {
                Query = @$"
                     query GetFooterContent($locale: I18NLocaleCode!){{
                      {contentPageKey}(locale: $locale) {{
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
    }
}
