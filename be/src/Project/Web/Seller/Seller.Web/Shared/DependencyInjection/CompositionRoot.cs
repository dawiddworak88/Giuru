using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Shared.Headers.ModelBuilders;
using Foundation.Extensions.ModelBuilders;
using Seller.Web.Shared.Footers.ModelBuilders;
using Seller.Web.Shared.MenuTiles.ModelBuilders;
using Foundation.PageContent.MenuTiles.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using System.Collections.Generic;
using Foundation.PageContent.Components.DrawerMenu.ViewModels;
using Seller.Web.Shared.DrawerMenu.ModelBuilders;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Areas.Products.ModelBuilders;
using Seller.Web.Shared.Repositories.Clients;
using Seller.Web.Shared.Repositories.Organisations;

namespace Seller.Web.Shared.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<IOrganisationsRepository, OrganisationsRepository>();
            services.AddScoped<IClientsRepository, ClientsRepository>();
            services.AddScoped<IModelBuilder<HeaderViewModel>, HeaderModelBuilder>();
            services.AddScoped<IModelBuilder<MenuTilesViewModel>, MenuTilesModelBuilder>();
            services.AddScoped<IModelBuilder<IEnumerable<DrawerMenuViewModel>>, DrawerMenuModelBuilder>();
            services.AddScoped<ICatalogModelBuilder, CatalogModelBuilder>();
            services.AddScoped<IModelBuilder<FooterViewModel>, FooterModelBuilder>();
            services.AddScoped<IModelBuilder<LogoViewModel>, LogoModelBuilder>();
        }
    }
}