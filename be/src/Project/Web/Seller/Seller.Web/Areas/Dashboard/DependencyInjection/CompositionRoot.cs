using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Areas.Dashboard.ModelBuilders;
using Seller.Web.Areas.Dashboard.Repositories;
using Seller.Web.Areas.Dashboard.ViewModels;

namespace Seller.Web.Areas.Dashboard.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDashboardAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, DashboardPageViewModel>, DashboardPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, DashboardDetailViewModel>, DashboardDetailModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, DailySalesAnalyticsViewModel>, DailySalesAnalyticsModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CountrySalesAnalyticsViewModel>, CountriesSalesAnalyticsModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ProductsSalesAnalyticsViewModel>, ProductsSalesAnalyticsModelBuilder>();

            services.AddScoped<ISalesAnalyticsRepository, SalesAnalyticsRepository>();
        }
    }
}
