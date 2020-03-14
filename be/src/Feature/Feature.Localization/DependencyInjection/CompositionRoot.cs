using Feature.Localization.ModelBuilders;
using Feature.Localization.ViewModels;
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
