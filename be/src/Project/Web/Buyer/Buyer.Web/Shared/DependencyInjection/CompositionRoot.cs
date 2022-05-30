using Microsoft.Extensions.DependencyInjection;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.Configuration;
using Buyer.Web.Shared.Configurations;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Buyer.Web.Shared.ViewModels.Headers;
using Buyer.Web.Areas.Home.DependencyInjection;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Buyer.Web.Areas.Products.DependencyInjection;
using Buyer.Web.Shared.Services.Catalogs;
using Buyer.Web.Shared.ModelBuilders.Catalogs;
using Buyer.Web.Shared.ComponentModels.Files;
using Buyer.Web.Shared.ModelBuilders.Breadcrumbs;
using Foundation.Localization.Definitions;
using Buyer.Web.Shared.ModelBuilders.MainNavigations;
using Buyer.Web.Shared.ModelBuilders.Footers;
using Buyer.Web.Shared.ModelBuilders.Headers;
using Buyer.Web.Shared.ViewModels.Files;
using Buyer.Web.Shared.ModelBuilders.Files;
using Buyer.Web.Shared.Repositories.Brands;
using Buyer.Web.Shared.Repositories.Products;
using Buyer.Web.Shared.Services.ContentDeliveryNetworks;
using Buyer.Web.Shared.Repositories.Clients;
using Buyer.Web.Shared.ViewModels.Sidebar;
using Buyer.Web.Shared.ModelBuilders.Sidebar;
using Buyer.Web.Shared.Services.Baskets;
using Buyer.Web.Shared.ViewModels.Modals;
using Buyer.Web.Shared.ModelBuilders.Modal;
using Buyer.Web.Shared.Repositories.News;
using Buyer.Web.Shared.Repositories.Files;
using Buyer.Web.Areas.Products.Repositories.Files;

namespace Buyer.Web.Shared.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.RegisterHomeDependencies();
            services.RegisterProductDependencies();

            // Model Builders
            services.AddScoped(typeof(ICatalogModelBuilder<,>), typeof(CatalogModelBuilder<,>));
            services.AddScoped(typeof(IBreadcrumbsModelBuilder<,>), typeof(BreadcrumbsModelBuilder<,>));
            services.AddScoped<IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel>, FilesModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel>, SidebarModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel>, ModalModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel>, HeaderModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel>, MainNavigationModelBuilder>();
            services.AddScoped<IModelBuilder<FooterViewModel>, FooterModelBuilder>();
            services.AddScoped<IModelBuilder<LogoViewModel>, LogoModelBuilder>();

            // Repositories
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<ICatalogProductsRepository, CatalogProductsRepository>();

            // Services
            services.AddScoped<ICdnService, CdnService>();
            services.AddScoped<ICatalogService, CatalogService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<IFilesRepository, FilesRepository>();
            services.AddScoped<IMediaItemsRepository, MediaItemsRepository>();

            // Client
            services.AddScoped<ICatalogOrderModelBuilder, CatalogOrderModelBuilder>();
            services.AddScoped<IClientsRepository, ClientsRepository>();
        }

        public static void ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration);
            services.Configure<LocalizationSettings>(configuration);
        }
    }
}
