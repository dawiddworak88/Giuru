﻿using Buyer.Web.Areas.Dashboard.ModelBuilders;
using Buyer.Web.Areas.Dashboard.Repositories;
using Buyer.Web.Areas.Dashboard.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;

namespace Buyer.Web.Areas.Dashboard.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDashboardAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<ISalesAnalyticsRepository, SalesAnalyticsRepository>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, DashboardPageViewModel>, DashboardPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, OrdersAnalyticsDetailViewModel>, OrdersAnalyticsDetailModelBuilder>();
        }
    }
}