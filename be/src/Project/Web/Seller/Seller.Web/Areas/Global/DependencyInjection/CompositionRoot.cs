using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Areas.Global.DomainModels;
using Seller.Web.Areas.Global.ModelBuilders;
using Seller.Web.Areas.Global.Repositories;
using Seller.Web.Areas.Global.ViewModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Global.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterGlobalAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICountriesRepository, CountriesRepository>();
            services.AddScoped<ICurrenciesRepository, CurrenciesRepository>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Country>>, CountriesPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CountriesPageViewModel>, CountriesPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CountryPageViewModel>, CountryPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CountryFormViewModel>, CountryFormModelBuilder>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Currency>>, CurrenciesPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CurrenciesPageViewModel>, CurrenciesPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CurrencyPageViewModel>, CurrencyPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CurrencyFormViewModel>, CurrencyFormModelBuilder>();
        }
    }
}
