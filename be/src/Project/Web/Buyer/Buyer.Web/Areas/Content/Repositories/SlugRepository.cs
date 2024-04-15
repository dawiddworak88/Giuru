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
    public class SlugRepository : ISlugRepository
    {
        private readonly IGraphQLClient _graphQLClient;
        private readonly ILogger<SlugRepository> _logger;

        public SlugRepository(
            IGraphQLClient graphQLClient,
            ILogger<SlugRepository> logger) 
        { 
            _graphQLClient = graphQLClient;
            _logger = logger;
        }

        public async Task<Slug> GetPageBySlugAsync(string language, string fallbackLanguage, string slug)
        {
            try
            {
                var response = await _graphQLClient.SendQueryAsync<JObject>(GetPageBySlugQuery(language, slug));

                if (response.Data is not null && response.Data["landingPages"]["data"].OrEmptyIfNull().Any() is false)
                {
                    response = await _graphQLClient.SendQueryAsync<JObject>(GetPageBySlugQuery(fallbackLanguage, slug));
                }

                var slugPageResponse = JsonConvert.DeserializeObject<SlugPageGraphQlResponseModel>(response.Data.ToString());

                var t = new Slug
                {
                    Title = slugPageResponse?.Test?.Data.FirstOrDefault()?.Attributes?.Title,
                };

                var slugPageBlocks = new List<BlockPage>();

                foreach (var block in slugPageResponse?.Test?.Data.FirstOrDefault()?.Attributes?.Blocks.OrEmptyIfNull())
                {
                    switch (block.TypeName)
                    {
                        case "ComponentSharedSlider":
                            if (block is ComponentSharedSlider sliderBlock)
                            {
                                var newSliderBlock = new SliderBlockPage
                                {
                                    HasNavigation = sliderBlock.HasNavigation,
                                    Skus = sliderBlock.Skus,
                                    Title = sliderBlock.Title,
                                    Typename = sliderBlock.TypeName
                                };

                                slugPageBlocks.Add(newSliderBlock);
                            }
                            break;

                        case "ComponentSharedContent":
                            if (block is ComponentSharedContent contentBlock)
                            {
                                var newContentBlock = new ContentBlockPage
                                {
                                    Typename = contentBlock.TypeName,
                                    Content = JsonConvert.SerializeObject(contentBlock.Content)
                                };

                                slugPageBlocks.Add(newContentBlock);
                            }
                            break;

                        default:
                            break;
                    }
                }

                t.Blocks = slugPageBlocks;

                return t;

            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Couldn't get slug {slug} page in language {language}");
            }

            return default;
        }

        private GraphQLRequest GetPageBySlugQuery(string language, string slug)
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
                switch (block.TypeName)
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
