using Buyer.Web.Areas.Home.GraphQlResponseModels;
using Buyer.Web.Areas.Home.RepositoryModels;
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

        public async Task<IEnumerable<HeroSliderItem>> GetHeroSliderItemsAsync(string language, string fallbackLanguage)
        {
            try
            {
                var response = await this.graphQlClient.SendQueryAsync<HomePageHeroSliderGraphQlResponseModel>(this.GetHomePageHeroSliderQuery(language));

                if (response?.Data?.HomePage?.Data?.Attributes.HeroSlider?.HeroSliderItems is null && string.IsNullOrWhiteSpace(fallbackLanguage) is false)
                {
                    response = await this.graphQlClient.SendQueryAsync<HomePageHeroSliderGraphQlResponseModel>(this.GetHomePageHeroSliderQuery(fallbackLanguage));
                }

                return response?.Data?.HomePage?.Data?.Attributes.HeroSlider?.HeroSliderItems?.Select(x => new HeroSliderItem
                {
                    CtaText = x.Cta?.Label,
                    CtaUrl = x.Cta?.Href?.ToString(),
                    TeaserTitle = x.Title,
                    TeaserText = x.Text,
                    Image = new HeroSliderItemImage
                    {
                        ImageSrc = x.Image?.Data?.Attributes?.Url?.ToString(),
                        ImageAlt = x.Image?.Data?.Attributes?.AlternativeText,
                        ImageTitle = x.Image?.Data?.Attributes?.Caption
                    }

                });
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, $"Couldn't get content for home page hero slider in language ${language}");
            }

            return default;
        }

        private GraphQLRequest GetHomePageHeroSliderQuery(string language)
        {
            return new GraphQLRequest
            {
                Query = @$"
                     query GetHomePageHeroSlider($locale: I18NLocaleCode!){{
                      homePage(locale: $locale) {{
		                data {{
                          id,
                          attributes {{
      	                    heroSlider {{
                              heroSliderItems
                              {{
                                title,
                                text,
                                cta {{
                                  href,
                                  label,
                                  target
                                }},
                                image {{
                                  data {{
                                    attributes {{
                                      url,
                                      alternativeText,
                                      caption
                                    }}
                                  }}
                                }}
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
