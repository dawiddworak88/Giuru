﻿using Microsoft.Extensions.DependencyInjection;
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
using Buyer.Web.Shared.Repositories.Clients;
using Buyer.Web.Shared.ViewModels.Sidebar;
using Buyer.Web.Shared.ModelBuilders.Sidebar;
using Buyer.Web.Shared.Services.Baskets;
using Buyer.Web.Shared.ViewModels.Modals;
using Buyer.Web.Shared.ModelBuilders.Modal;
using Buyer.Web.Shared.Repositories.News;
using Buyer.Web.Shared.Repositories.Files;
using Buyer.Web.Areas.Products.Repositories.Files;
using Foundation.Media.Configurations;
using Buyer.Web.Shared.ViewModels.OrderItemStatusChanges;
using Buyer.Web.Shared.ModelBuilders.OrderItemStatusChanges;
using Buyer.Web.Shared.Repositories.GraphQl;
using Buyer.Web.Shared.ModelBuilders.NotificationBar;
using Buyer.Web.Shared.ViewModels.NotificationBar;
using Buyer.Web.Shared.ViewModels.Headers.Search;
using Buyer.Web.Shared.ModelBuilders.Headers.Search;
using Buyer.Web.Shared.ViewModels.Headers.UserPopup;
using Buyer.Web.Shared.ViewModels.Headers.SidebarMobile;
using Buyer.Web.Shared.ModelBuilders.Headers.UserPopup;
using Buyer.Web.Shared.ModelBuilders.Headers.SidebarMobile;

namespace Buyer.Web.Shared.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterHomeDependencies();
            services.RegisterProductDependencies();

            // Model Builders
            services.AddScoped(typeof(ICatalogModelBuilder<,>), typeof(CatalogModelBuilder<,>));
            services.AddScoped(typeof(IBreadcrumbsModelBuilder<,>), typeof(BreadcrumbsModelBuilder<,>));
            services.AddScoped<IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel>, FilesModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<FilesComponentModel, DownloadCenterFilesViewModel>, DownloadCenterFilesModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel>, SidebarModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel>, ModalModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel>, BuyerHeaderModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel>, MainNavigationModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OrderItemStatusChangesViewModel>, OrderItemStatusChangesModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel>, FooterModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, NotificationBarViewModel>, NotificationBarModelBuilder>();
            services.AddScoped<IModelBuilder<LogoViewModel>, LogoModelBuilder>();
            services.AddScoped<IModelBuilder<HeaderViewModel>, HeaderModelBuilder>();

            services.AddScoped<IModelBuilder<SearchViewModel>, SearchModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, SidebarMobileViewModel>, SidebarMobileModelBuilder>();
            services.AddScoped<IComponentModelBuilder<ComponentModelBase, UserPopupViewModel>, UserPopupModelBuilder>();

            // Repositories
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<ICatalogProductsRepository, CatalogProductsRepository>();
            services.AddScoped<IGraphQlRepository, GraphQlRepository>();
            services.AddScoped<IClientAddressesRepository, ClientAddressesRepository>();

            // Services
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
            services.Configure<MediaAppSettings>(configuration);
        }
    }
}
