using Buyer.Web.Areas.Home.DomainModels;
using Buyer.Web.Areas.Home.GraphQlResponseModels;
using Foundation.Extensions.ExtensionMethods;
using GraphQL;
using GraphQL.Client.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Home.Repositories
{
    public class HeroSliderRepository : IHeroSliderRepository
    {
        private readonly IGraphQLClient graphQlClient;
        private readonly ILogger<HeroSliderRepository> logger;

        public HeroSliderRepository(
            IGraphQLClient graphQlClient,
            ILogger<HeroSliderRepository> logger)
        {
            this.graphQlClient = graphQlClient;
            this.logger = logger;
        }

        public async Task<IEnumerable<HeroSliderItem>> GetHeroSliderItemsAsync(string language)
        {
            try
            {
                var query = new GraphQLRequest
                {
                    Query = @$"
                     query GetHomePageHeroSlider{{
                      homePage(locale: ""{language}"") {{
		                data {{
                          id,
                          attributes {{
      	                    heroSlider {{
                              heroSliderItems
                              {{
                                title,
                                link {{
                                  href,
                                  label,
                                  target
                                }},
                                media {{
                                  data {{
                                    attributes {{
                                      url,
                                      alternativeText,
                                      caption,
                                      mime,
                                      name
                                    }}
                                  }}
                                }}
                              }}
                            }}
                        }}
	                    }}
                      }}
                    }}"
                };

                var response = await this.graphQlClient.SendQueryAsync<HomePageHeroSliderGraphQlResponseModel>(query);

                return response?.Data?.HomePage?.Data?.Attributes.HeroSlider?.HeroSliderItems.OrEmptyIfNull().Select(x => new HeroSliderItem
                {
                    CtaText = x.Title,
                    CtaUrl = x.Link?.Href?.ToString(),
                    TeaserTitle = string.Empty,
                    TeaserText = string.Empty,
                    Image = new HeroSliderItemImage
                    {
                        ImageSrc = x.Media?.Data?.Attributes?.Url?.ToString()
                    }

                });
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, $"Couldn't get content for home page hero slider in language ${language}");
            }

            return default;
        }
    }
}
