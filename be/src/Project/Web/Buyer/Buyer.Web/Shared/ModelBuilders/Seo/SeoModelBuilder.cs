using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.DomainModels.Metadata;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Seo.ViewModels;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.ModelBuilders.Seo
{
    public class SeoModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, SeoViewModel>
    {
        private readonly IOptionsMonitor<AppSettings> options;

        public SeoModelBuilder(IOptionsMonitor<AppSettings> options)
        {
            this.options = options;
        }

        public async Task<SeoViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var graphQLClient = new GraphQLHttpClient(this.options.CurrentValue.ContentGraphQlUrl, new NewtonsoftJsonSerializer());
            graphQLClient.HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer 2c994cdbc32d6257d3529b5b767359364672c53548a6f324ff5cf3a76b3826c52ff159272732cd4627b60cd002005b785e8d98d29a90acdc527d11fba1e1c7586166ee785c3697b039fb29d84e7ea11c43bbed7c1101c7bf50f772c340a31356bea2dfbec174bcce34505bc8d43897eb2906cdcaf0e4a9dbc74fa6b2af1c2e25");


            var seoRequest = new GraphQLRequest
            {
                Query = @"
                 query {
                  homePage {
		            data {
                      id,
                      attributes {
                        seo {
                          metaTitle,
                          metaDescription
                        }
                      }
	                }
                  }
                }"
            };

            var graphQLResponse = await graphQLClient.SendQueryAsync<dynamic>(seoRequest);

            return new SeoViewModel
            { 
                MetaTitle = string.Empty,
                MetaDescription = string.Empty
            };
        }
    }
}
