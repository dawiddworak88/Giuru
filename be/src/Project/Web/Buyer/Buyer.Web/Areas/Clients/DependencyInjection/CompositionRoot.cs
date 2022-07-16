using Buyer.Web.Areas.Clients.ModelBuilders;
using Buyer.Web.Areas.Clients.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;

namespace Buyer.Web.Areas.Clients.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterClientsDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ApplicationPageViewModel>, ApplicationPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, ApplicationFormViewModel>, ApplicationFormModelBuilder>();
        }
    }
}
