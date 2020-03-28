using Feature.PageContent.Components.LanguageSwitchers.ModelBuilders;
using Feature.PageContent.Components.LanguageSwitchers.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.DependencyInjection;

namespace Feature.PageContent.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterLocalizationDependencies(this IServiceCollection services)
        {
            services.AddScoped<IModelBuilder<LanguageSwitcherViewModel>, LanguageSwitcherModelBuilder>();
        }
    }
}
