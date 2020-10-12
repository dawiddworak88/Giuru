using Identity.Api.Areas.Accounts.ComponentModels;
using Identity.Api.Areas.Accounts.ModelBuilders;
using Identity.Api.Areas.Accounts.ViewModels;
using Identity.Api.ModelBuilders.SignInForm;
using Identity.Api.Shared.Footers.ModelBuilders;
using Identity.Api.Shared.Headers.ModelBuilders;
using Identity.Api.ViewModels.SignInForm;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Identity.Api.Shared.Configurations;

namespace Identity.Api.Shared.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<IComponentModelBuilder<SignInComponentModel, SignInViewModel>, SignInModelBuilder>();
            services.AddScoped<IComponentModelBuilder<SignInFormComponentModel, SignInFormViewModel>, SignInFormModelBuilder>();
            services.AddScoped<IModelBuilder<HeaderViewModel>, HeaderModelBuilder>();
            services.AddScoped<IModelBuilder<FooterViewModel>, FooterModelBuilder>();
            services.AddScoped<IModelBuilder<LogoViewModel>, LogoModelBuilder>();
        }

        public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration);
        }
    }
}
