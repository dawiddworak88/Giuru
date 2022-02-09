using Buyer.Web.Areas.Outlet.ModelBuilders;
using Buyer.Web.Areas.Outlet.Repositories;
using Buyer.Web.Areas.Outlet.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;

namespace Buyer.Web.Areas.Outlet.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterOutletDependencies(this IServiceCollection services)
        {
            services.AddScoped<IOutletRepository, OutletRepository>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OutletPageViewModel>, OutletPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OutletPageCatalogViewModel>, OutletCatalogModelBuilder>();
        }
    }
}
