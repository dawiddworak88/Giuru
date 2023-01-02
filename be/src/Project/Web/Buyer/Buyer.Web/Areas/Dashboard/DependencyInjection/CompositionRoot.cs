using Buyer.Web.Areas.Dashboard.ModelBuilders;
using Buyer.Web.Areas.Dashboard.Repositories.Identity;
using Buyer.Web.Areas.Dashboard.ViewModel;
using Buyer.Web.Areas.Dashboard.Repositories;
using Buyer.Web.Areas.Dashboard.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;

namespace Buyer.Web.Areas.Dashboard.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDashboardDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, DashboardPageViewModel>, DashboardPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OrdersAnalyticsDetailViewModel>, OrdersAnalyticsDetailModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, SalesAnalyticsViewModel>, SalesAnalyticsModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, SettingsPageViewModel>, SettingsPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, SettingsFormViewModel>, SettingsFormModelBuilder>();

            services.AddScoped<IIdentityRepository, IdentityRepository>();
            services.AddScoped<ISalesAnalyticsRepository, SalesAnalyticsRepository>();
        }
    }
}
