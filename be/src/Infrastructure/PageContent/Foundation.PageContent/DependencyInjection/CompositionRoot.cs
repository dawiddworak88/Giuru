using Foundation.PageContent.Components.LanguageSwitchers.ModelBuilders;
using Foundation.PageContent.Components.LanguageSwitchers.ViewModels;
using Foundation.PageContent.Services.MetaTags;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.PageContent.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterLocalizationDependencies(this IServiceCollection services)
        {
            services.AddScoped<IModelBuilder<LanguageSwitcherViewModel>, LanguageSwitcherModelBuilder>();
            services.AddScoped<IMetaTagsService, MetaTagsService>();
        }
    }
}
