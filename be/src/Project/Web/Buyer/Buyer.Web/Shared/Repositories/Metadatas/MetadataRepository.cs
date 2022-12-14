using Buyer.Web.Shared.DomainModels.Metadata;
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

namespace Buyer.Web.Shared.Repositories.Metadatas
{
    public class MetadataRepository : IMetadataRepository
    {
        private readonly IGraphQLClient graphQlClient;
        private readonly ILogger<MetadataRepository> logger;

        public MetadataRepository(
            IGraphQLClient graphQlClient,
            ILogger<MetadataRepository> logger)
        {
            this.graphQlClient = graphQlClient;
            this.logger = logger;
        }

        public async Task<Metadata> GetMetadataAsync(string contentPageKey, string language)
        {
            try
            {
                var query = new GraphQLRequest
                {
                    Query = @$"
                     query GetMetadata{{
                      {contentPageKey}(locale: ""{language}"") {{
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

                var response = await this.graphQlClient.SendQueryAsync<JObject>(query);

                if (response.Errors.OrEmptyIfNull().Any() is false && response?.Data != null)
                {
                    var replacedContentPageKey = response.Data.ToString().Replace(contentPageKey, "page");

                    var metaData = JsonConvert.DeserializeObject<SeoGraphQlResponseModel>(replacedContentPageKey);

                    return new Metadata
                    {
                        MetaTitle = metaData?.Page?.Data?.Attributes?.Seo?.MetaTitle,
                        MetaDescription = metaData?.Page?.Data?.Attributes?.Seo?.MetaDescription
                    };
                }
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, $"Couldn't get content for ${contentPageKey} in language ${language}");
            }

            return default;
        }
    }
}
