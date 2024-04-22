using GraphQL;
using GraphQL.Client.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Foundation.Extensions.ExtensionMethods;
using System.Linq;
using Identity.Api.Areas.Home.ResponseModels;

namespace Identity.Api.Areas.Home.Repositories.Content
{
    public class ContentRepository : IContentRepository
    {
        private readonly IGraphQLClient _graphQlClient;
        private readonly ILogger<ContentRepository> _logger;

        public ContentRepository(
            IGraphQLClient graphQLClient,
            ILogger<ContentRepository> logger)
        {
            _graphQlClient = graphQLClient;
            _logger = logger;
        }

        public async Task<DomainModels.Content> GetContentAsync(string contentPageKey, string language)
        {
            try
            {
                var query = new GraphQLRequest
                {
                    Query = $@"query getContent {{
                      {contentPageKey}(locale: ""{language}"") {{
                        data {{
                          id
                          attributes {{
                            content {{
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
                    var replacedContentPageKey = response.Data.ToString().Replace(contentPageKey, "page");

                    var metaData = JsonConvert.DeserializeObject<ContentGraphQlResponseModel>(replacedContentPageKey);

                    return new DomainModels.Content
                    {
                        Title = metaData.Page?.Data?.Attributes?.Content?.Title,
                        Description = metaData.Page?.Data?.Attributes?.Content?.Description,
                        Accordions = metaData.Page?.Data?.Attributes?.Content?.Accordion?.AccordionItems.Select(x => new DomainModels.AccordionItem
                        {
                            Title = x.Title,
                            Description = x.Description
                        })
                    };
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Couldn't get content for ${contentPageKey} in language ${language}");
            }

            return default;
        }
    }
}
