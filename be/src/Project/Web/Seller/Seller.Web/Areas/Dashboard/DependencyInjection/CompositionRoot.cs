using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Areas.Dashboard.ModelBuilders;
using Seller.Web.Areas.Dashboard.ViewModels;

namespace Seller.Web.Areas.Dashboard.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDashboardAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, DashboardPageViewModel>, DashboardPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, DashboardDetailViewModel>, DashboardDetailModelBuilder>();
        }
    }
}
