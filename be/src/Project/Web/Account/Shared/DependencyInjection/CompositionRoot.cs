using Account.Areas.Accounts.ComponentModels;
using Account.Areas.Accounts.ModelBuilders;
using Account.Areas.Accounts.ViewModels;
using Account.ModelBuilders.SignInForm;
using Account.Shared.Footers.ModelBuilders;
using Account.Shared.Headers.ModelBuilders;
using Account.ViewModels.SignInForm;
using Feature.PageContent.Components.Footers.ViewModels;
using Feature.PageContent.Components.Headers.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.DependencyInjection;

namespace Account.Shared.DependencyInjection
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
    }
}
