using Foundation.Extensions.ExtensionMethods;
using Foundation.PageContent.DomainModels.Metadatas;
using Foundation.PageContent.ResponseModels.Seo;
using GraphQL;
using GraphQL.Client.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.PageContent.Repositories.Metadatas
{
    public class MetadataRepository : IMetadataRepository
    {
        private readonly IGraphQLClient _graphQlClient;
        private readonly ILogger<MetadataRepository> _logger;

        public MetadataRepository(
            IGraphQLClient graphQlClient,
            ILogger<MetadataRepository> logger)
        {
            _graphQlClient = graphQlClient;
            _logger = logger;
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

                var response = await _graphQlClient.SendQueryAsync<JObject>(query);

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
                _logger.LogError(exception, $"Couldn't get content for ${contentPageKey} in language ${language}");
            }

            return default;
        }
    }
}
