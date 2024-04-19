using Buyer.Web.Areas.Content.DomainModel;
using Buyer.Web.Areas.Content.GraphQlResponseModels;
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

namespace Buyer.Web.Areas.Content.Repositories
{
    public class ContentRepository : IContentRepository
    {
        private readonly IGraphQLClient _graphQLClient;
        private readonly ILogger<ContentRepository> _logger;

        public ContentRepository(
            IGraphQLClient graphQLClient,
            ILogger<ContentRepository> logger) 
        { 
            _graphQLClient = graphQLClient;
            _logger = logger;
        }

        public async Task<DomainModel.Content> GetContentPageBySlugAsync(string language, string fallbackLanguage, string slug)
        {
            try
            {
                var response = await _graphQLClient.SendQueryAsync<JObject>(GetContentPageBySlugQuery(language, slug));

                if (response.Data is not null && response.Data["landingPages"]["data"].OrEmptyIfNull().Any() is false)
                {
                    response = await _graphQLClient.SendQueryAsync<JObject>(GetContentPageBySlugQuery(fallbackLanguage, slug));
                }

                var contentPageResponse = JsonConvert.DeserializeObject<SlugPageGraphQlResponseModel>(response.Data.ToString());

                var contentPage = new DomainModel.Content
                {
                    Title = contentPageResponse?.LandingPage?.Data.FirstOrDefault()?.Attributes?.Title,
                };

                var sharedComponents = new List<SharedComponent>();

                foreach (var sharedComponent in contentPageResponse?.LandingPage?.Data.FirstOrDefault()?.Attributes?.Blocks.OrEmptyIfNull())
                {
                    switch (sharedComponent.Typename)
                    {
                        case "ComponentSharedSlider":
                            if (sharedComponent is ComponentSharedSlider sharedSlider)
                            {
                                var sliderComponent = new SharedSliderComponent
                                {
                                    HasNavigation = sharedSlider.HasNavigation,
                                    Skus = sharedSlider.Skus,
                                    Title = sharedSlider.Title,
                                    Typename = sharedSlider.Typename
                                };

                                sharedComponents.Add(sliderComponent);
                            }
                            break;
                        case "ComponentSharedContent":
                            if (sharedComponent is ComponentSharedContent sharedContent)
                            {
                                var contentComponent = new SharedContentComponent
                                {
                                    Typename = sharedContent.Typename,
                                    Content = JsonConvert.SerializeObject(sharedContent.Content)
                                };

                                sharedComponents.Add(contentComponent);
                            }
                            break;
                        default:
                            break;
                    }
                }

                contentPage.SharedComponents = sharedComponents;

                return contentPage;

            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Couldn't get slug page ({slug}) in language {language}");
            }

            return default;
        }

        private GraphQLRequest GetContentPageBySlugQuery(string language, string slug)
        {
            return new GraphQLRequest
            {
                Query = $@"
                    query SlugPage($locale: I18NLocaleCode, $slug: String) {{
                      landingPages(locale: $locale, filters: {{ slug: {{ eq: $slug }}}}) {{
                        data {{
                          attributes {{
                            slug
                            title
                            seo {{
                              metaTitle
                              metaDescription
                            }}
                            blocks {{
                              __typename,
                              ... on ComponentSharedContent {{
                                id,
                                content
                              }}
                              ... on ComponentSharedSlider {{
                                id
                                title
                                navigation
                                skus
                              }}
                            }}
                          }}
                        }}
                      }}
                    }}
                ",
                Variables = new
                {
                    locale = language,
                    slug
                }
            };
        }
    }

    public class BlockConverter : JsonConverter<IEnumerable<Block>>
    {
        public override IEnumerable<Block> ReadJson(JsonReader reader, Type objectType, IEnumerable<Block> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jArray = JArray.Load(reader);
            var result = new List<Block>();

            foreach (var token in jArray)
            {
                var block = token.ToObject<Block>(serializer);

                switch (block.Typename)
                {
                    case "ComponentSharedSlider":
                        result.Add(token.ToObject<ComponentSharedSlider>(serializer));
                        break;
                    case "ComponentSharedContent":
                        result.Add(token.ToObject<ComponentSharedContent>(serializer));
                        break;
                    // Add cases for other block types as needed
                    default:
                        result.Add(block);
                        break;
                }
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, IEnumerable<Block> value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
