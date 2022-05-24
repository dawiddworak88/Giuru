using Buyer.Web.Areas.Dashboard.ModelBuilders;
using Buyer.Web.Areas.Dashboard.Repositories.Identity;
using Buyer.Web.Areas.Dashboard.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;

namespace Buyer.Web.Areas.Dashboard.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDashboardDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, SettingsPageViewModel>, SettingsPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, SettingsFormViewModel>, SettingsFormModelBuilder>();

            services.AddScoped<IIdentityRepository, IdentityRepository>();
        }
    }
}
