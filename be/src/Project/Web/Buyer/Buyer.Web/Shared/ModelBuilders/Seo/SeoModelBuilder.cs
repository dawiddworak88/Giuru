using Buyer.Web.Shared.DomainModels.Metadata;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Seo.ViewModels;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.ModelBuilders.Seo
{
    public class SeoModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, SeoViewModel>
    {
        public async Task<SeoViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var graphQLClient = new GraphQLHttpClient("http://content-api:5111/graphql", new NewtonsoftJsonSerializer());

            var seoRequest = new GraphQLRequest
            {
                Query = @"
                {
                    home-page {
                        seo {
                            metaTitle,
                            metaDescription
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
