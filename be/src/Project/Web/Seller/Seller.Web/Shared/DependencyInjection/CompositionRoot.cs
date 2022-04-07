using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Shared.ModelBuilders.Headers;
using Foundation.Extensions.ModelBuilders;
using Seller.Web.Shared.ModelBuilders.Footers;
using Seller.Web.Shared.ModelBuilders.MenuTiles;
using Foundation.PageContent.MenuTiles.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using System.Collections.Generic;
using Foundation.PageContent.Components.DrawerMenu.ViewModels;
using Seller.Web.Shared.ModelBuilders.DrawerMenu;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Areas.ModelBuilders.Products;
using Seller.Web.Shared.Repositories.Clients;
using Seller.Web.Shared.Repositories.Organisations;
using Seller.Web.Areas.Shared.Repositories.Products;
using Seller.Web.Shared.Repositories.Identity;
using Seller.Web.Shared.Services.ContentDeliveryNetworks;
using Seller.Web.Areas.Shared.Repositories.Media;

namespace Seller.Web.Shared.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICdnService, CdnService>();
            services.AddScoped<IMediaItemsRepository, MediaItemsRepository>();
            services.AddScoped<IOrganisationsRepository, OrganisationsRepository>();
            services.AddScoped<IClientsRepository, ClientsRepository>();
            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<IIdentityRepository, IdentityRepository>();
            services.AddScoped<IModelBuilder<HeaderViewModel>, HeaderModelBuilder>();
            services.AddScoped<IModelBuilder<MenuTilesViewModel>, MenuTilesModelBuilder>();
            services.AddScoped<IModelBuilder<IEnumerable<DrawerMenuViewModel>>, DrawerMenuModelBuilder>();
            services.AddScoped<ICatalogModelBuilder, CatalogModelBuilder>();
            services.AddScoped<IModelBuilder<FooterViewModel>, FooterModelBuilder>();
            services.AddScoped<IModelBuilder<LogoViewModel>, LogoModelBuilder>();
        }
    }
}