using Feature.PageContent.Shared.LanguageSwitchers.ModelBuilders;
using Feature.PageContent.Shared.LanguageSwitchers.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.DependencyInjection;

namespace Feature.Localization.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterLocalizationDependencies(this IServiceCollection services)
        {
            services.AddScoped<IModelBuilder<LanguageSwitcherViewModel>, LanguageSwitcherModelBuilder>();
        }
    }
}
