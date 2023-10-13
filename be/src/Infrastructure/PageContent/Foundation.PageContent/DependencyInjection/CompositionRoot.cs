using Foundation.PageContent.Components.LanguageSwitchers.ModelBuilders;
using Foundation.PageContent.Components.LanguageSwitchers.ViewModels;
using Foundation.PageContent.Services.MetaTags;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.DependencyInjection;
using Foundation.PageContent.Repositories.Metadatas;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Metadatas.ViewModels;
using Foundation.PageContent.ModelBuilders.Metadatas;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Extensions.Configuration;

namespace Foundation.PageContent.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterLocalizationDependencies(this IServiceCollection services)
        {
            services.AddScoped<IModelBuilder<LanguageSwitcherViewModel>, LanguageSwitcherModelBuilder>();
            services.AddScoped<IMetaTagsService, MetaTagsService>();
        }

        public static void RegisterStrapiDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMetadataRepository, MetadataRepository>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel>, MetadataModelBuilder>();

            // GraphQL
            services.AddScoped<IGraphQLClient>(sp =>
            {
                var graphQlClient = new GraphQLHttpClient(configuration["ContentGraphQlUrl"], new NewtonsoftJsonSerializer());
                graphQlClient.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {configuration["ContentGraphQlAuthorizationKey"]}");
                return graphQlClient;
            });
        }
    }
}
