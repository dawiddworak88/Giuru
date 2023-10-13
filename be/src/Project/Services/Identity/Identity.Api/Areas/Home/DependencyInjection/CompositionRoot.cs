using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Metadatas.ViewModels;
using Identity.Api.Areas.Home.ModelBuilders;
using Identity.Api.Areas.Home.Repositories.Content;
using Identity.Api.Areas.Home.Services.Contents;
using Identity.Api.Areas.Home.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Api.Areas.Home.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterHomeViewsDependencies(this IServiceCollection services)
        {
            services.AddScoped<IContentService, ContentService>();
            services.AddScoped<IContentRepository, ContentRepository>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, PrivacyPolicyPageViewModel>, PrivacyPolicyPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, RegulationsPageViewModel>, RegulationsPageModelBuilder>();
        }
    }
}
