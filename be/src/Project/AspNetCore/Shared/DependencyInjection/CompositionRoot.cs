using Microsoft.Extensions.DependencyInjection;
using AspNetCore.Shared.Headers.ModelBuilders;
using AspNetCore.Shared.Headers.ViewModels;
using AspNetCore.Areas.Home.ViewModel;
using AspNetCore.Areas.Home.ModelBuilders;
using Foundation.Extensions.ModelBuilders;
using AspNetCore.Shared.Footers.ViewModels;
using AspNetCore.Shared.Footers.ModelBuilders;

namespace AspNetCore.Shared.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<IModelBuilder<HomePageViewModel>, HomePageModelBuilder>();
            services.AddScoped<IModelBuilder<HeaderViewModel>, HeaderModelBuilder>();
            services.AddScoped<IModelBuilder<FooterViewModel>, FooterModelBuilder>();
            services.AddScoped<IModelBuilder<LogoViewModel>, LogoModelBuilder>();
        }
    }
}
