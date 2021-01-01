using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Areas.Settings.ModelBuilders;
using Seller.Web.Areas.Settings.ViewModels;

namespace Seller.Web.Areas.Settings.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterSettingsAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, SettingsPageViewModel>, SettingsPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, SettingsFormViewModel>, SettingsFormModelBuilder>();
        }
    }
}
