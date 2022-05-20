using Buyer.Web.Shared.GraphQlResponseModels;
using Foundation.Content.Factories.GraphQlFactories;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Seo.ViewModels;
using GraphQL;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.ModelBuilders.Seo
{
    public class SeoModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, SeoViewModel>
    {
        private readonly IGraphQlClientFactory graphQlClientFactory;

        public SeoModelBuilder(IGraphQlClientFactory graphQlClientFactory)
        {
            this.graphQlClientFactory = graphQlClientFactory;
        }

        public async Task<SeoViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var client = this.graphQlClientFactory.Get(componentModel.Language);

            var query = new GraphQLRequest
            {
                Query = @$"
                 query {{
                  {componentModel.ContentPageKey} {{
		            data {{
                      id,
                      attributes {{
                        seo {{
                          metaTitle,
                          metaDescription
                        }}
                      }}
	                }}
                  }}
                }}"
            };

            var response = await client.SendQueryAsync<SeoGraphQlResponseModel>(query);

            return new SeoViewModel
            { 
                MetaTitle = response?.Data?.HomePage?.Data?.Attributes?.Seo?.MetaTitle,
                MetaDescription = response?.Data?.HomePage?.Data?.Attributes?.Seo?.MetaDescription
            };
        }
    }
}
