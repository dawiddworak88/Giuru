using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Areas.Outlet.DomainModels;
using Seller.Web.Areas.Outlet.ModelBuilders;
using Seller.Web.Areas.Outlet.ViewModel;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Outlet.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterOutletAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<OutletItem>>, OutletPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OutletPageViewModel>, OutletPageModelBuilder>();
        }
    }
}
