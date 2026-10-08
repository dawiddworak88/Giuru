using Foundation.Pricing.Configurations;
using Foundation.Pricing.DiscountCodes;
using Foundation.Pricing.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using BuyerAppSettings = Buyer.Web.Shared.Configurations.AppSettings;
using SellerAppSettings = Seller.Web.Shared.Configurations.AppSettings;

namespace Giuru.UnitTests.Services.DiscountCodes
{
    // Resolves the real composition roots with scope validation on, so a captive dependency or an unresolved constructor
    // parameter (the validator, the lookup, the decorator, the Grula service) fails here instead of at the first request.
    public class DiscountCodeCompositionTests
    {
        private static IConfiguration CreateConfiguration(bool enforcementEnabled)
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["GrulaUrl"] = "http://localhost:5999",
                    ["GrulaAccessToken"] = "grula-token",
                    ["GrulaEnvironmentId"] = "00000000-0000-0000-0000-000000000001",
                    ["ClientUrl"] = "http://localhost:5001",
                    ["DiscountCodeEnforcementEnabled"] = enforcementEnabled.ToString()
                })
                .Build();
        }

        private static ServiceProvider BuildSellerProvider(IConfiguration configuration)
        {
            var services = new ServiceCollection();

            AddInfrastructureStubs(services);
            Foundation.ApiExtensions.DependencyInjection.CompositionRoot.RegisterApiExtensionsDependencies(services);
            Foundation.Media.DependencyInjection.CompositionRoot.RegisterFoundationMediaDependencies(services);
            Foundation.PageContent.DependencyInjection.CompositionRoot.RegisterLocalizationDependencies(services);
            Foundation.Extensions.DependencyInjection.CompositionRoot.RegisterGeneralDependencies(services);
            Seller.Web.Shared.DependencyInjection.CompositionRoot.RegisterDependencies(services, configuration);
            Seller.Web.Areas.Orders.DependencyInjection.CompositionRoot.RegisterOrdersAreaDependencies(services);
            Seller.Web.Areas.Clients.DependencyInjection.CompositionRoot.RegisterClientsAreaDependencies(services);
            Seller.Web.Areas.Inventory.DependencyInjection.CompositionRoot.RegisterInventoryAreaDependencies(services);
            Seller.Web.Areas.Products.DependencyInjection.CompositionRoot.RegisterProductsAreaDependencies(services);
            Seller.Web.Areas.Global.DependencyInjection.CompositionRoot.RegisterGlobalAreaDependencies(services);
            Seller.Web.Areas.Media.DependencyInjection.CompositionRoot.RegisterMediaAreaDependencies(services);
            Seller.Web.Shared.DependencyInjection.ConfigurationRoot.ConfigureSettings(services, configuration);

            // The real service opens a Redis database in its constructor and has nothing to do with discount codes.
            services.AddScoped(_ => Substitute.For<Seller.Web.Shared.Services.ProductColors.IProductColorsService>());

            return services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
        }

        private static ServiceProvider BuildBuyerProvider(IConfiguration configuration)
        {
            var services = new ServiceCollection();

            AddInfrastructureStubs(services);
            Foundation.ApiExtensions.DependencyInjection.CompositionRoot.RegisterApiExtensionsDependencies(services);
            Foundation.Media.DependencyInjection.CompositionRoot.RegisterFoundationMediaDependencies(services);
            Foundation.PageContent.DependencyInjection.CompositionRoot.RegisterLocalizationDependencies(services);
            Foundation.Extensions.DependencyInjection.CompositionRoot.RegisterGeneralDependencies(services);
            Buyer.Web.Areas.Orders.DependencyInjection.CompositionRoot.RegisterOrdersAreaDependencies(services);
            Buyer.Web.Areas.Clients.DependencyInjection.CompositionRoot.RegisterClientsDependencies(services);
            Buyer.Web.Areas.Dashboard.DependencyInjection.CompositionRoot.RegisterDashboardAreaDependencies(services);
            Buyer.Web.Areas.Content.DependencyInjection.CompositionRoot.RegisterContentDependencies(services);
            Buyer.Web.Areas.News.DependencyInjection.CompositionRoot.RegisterNewsDependencies(services);
            Buyer.Web.Areas.DownloadCenter.DependencyInjection.CompositionRoot.RegisterDownloadCenterDependencies(services);
            Buyer.Web.Shared.DependencyInjection.CompositionRoot.RegisterDependencies(services, configuration);
            Buyer.Web.Shared.DependencyInjection.CompositionRoot.ConfigureSettings(services, configuration);

            // The real service opens a Redis database in its constructor and has nothing to do with discount codes.
            services.AddScoped(_ => Substitute.For<Buyer.Web.Areas.Products.Services.ProductColors.IProductColorsService>());

            return services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
        }

        private static void AddInfrastructureStubs(IServiceCollection services)
        {
            services.AddLogging();
            services.AddOptions();
            services.AddHttpContextAccessor();
            services.AddRouting();
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddLocalization();
            services.AddSingleton(Substitute.For<IConnectionMultiplexer>());

        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void SellerGraph_ResolvesTheDiscountCodeServicesInAScope(bool enforcementEnabled)
        {
            using var provider = BuildSellerProvider(CreateConfiguration(enforcementEnabled));
            using var scope = provider.CreateScope();

            var validator = scope.ServiceProvider.GetRequiredService<IDiscountCodeValidator>();
            var lookup = scope.ServiceProvider.GetRequiredService<IDiscountCodeLookup>();
            var resolver = scope.ServiceProvider.GetRequiredService<IPriceClientResolver>();
            var grula = scope.ServiceProvider.GetRequiredService<IGrulaDiscountCodeService>();

            Assert.NotNull(validator);
            Assert.NotNull(lookup);
            Assert.NotNull(grula);

            // Whatever the setting, every pricing path resolves its client through the verifying decorator.
            Assert.IsType<VerifiedDiscountCodePriceClientResolver>(resolver);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void BuyerGraph_ResolvesTheDiscountCodeServicesInAScope(bool enforcementEnabled)
        {
            using var provider = BuildBuyerProvider(CreateConfiguration(enforcementEnabled));
            using var scope = provider.CreateScope();

            var validator = scope.ServiceProvider.GetRequiredService<IDiscountCodeValidator>();
            var lookup = scope.ServiceProvider.GetRequiredService<IDiscountCodeLookup>();
            var resolver = scope.ServiceProvider.GetRequiredService<IPriceClientResolver>();

            Assert.NotNull(validator);
            Assert.NotNull(lookup);
            Assert.IsType<VerifiedDiscountCodePriceClientResolver>(resolver);
        }

        [Fact]
        public void SellerGraph_ResolvesTheControllersThatAcceptADiscountCode()
        {
            using var provider = BuildSellerProvider(CreateConfiguration(true));
            using var scope = provider.CreateScope();

            Assert.NotNull(ActivatorUtilities.CreateInstance<Seller.Web.Areas.Orders.ApiControllers.BasketsApiController>(scope.ServiceProvider));
            Assert.NotNull(ActivatorUtilities.CreateInstance<Seller.Web.Areas.Orders.ApiControllers.OrderFileApiController>(scope.ServiceProvider));
            Assert.NotNull(ActivatorUtilities.CreateInstance<Seller.Web.Areas.Orders.ApiControllers.BasketCheckoutApiController>(scope.ServiceProvider));
            Assert.NotNull(ActivatorUtilities.CreateInstance<Seller.Web.Areas.Clients.ApiControllers.DiscountCodesApiController>(scope.ServiceProvider));
        }

        [Fact]
        public void BuyerGraph_ResolvesTheControllersThatAcceptADiscountCode()
        {
            using var provider = BuildBuyerProvider(CreateConfiguration(true));
            using var scope = provider.CreateScope();

            Assert.NotNull(ActivatorUtilities.CreateInstance<Buyer.Web.Areas.Orders.ApiControllers.BasketsApiController>(scope.ServiceProvider));
            Assert.NotNull(ActivatorUtilities.CreateInstance<Buyer.Web.Areas.Orders.ApiControllers.OrderFileApiController>(scope.ServiceProvider));
            Assert.NotNull(ActivatorUtilities.CreateInstance<Buyer.Web.Areas.Orders.ApiControllers.BasketCheckoutApiController>(scope.ServiceProvider));
        }

        [Fact]
        public void TheDriverIdCache_IsSharedAcrossScopes()
        {
            using var provider = BuildSellerProvider(CreateConfiguration(true));

            using var first = provider.CreateScope();
            using var second = provider.CreateScope();

            Assert.Same(
                first.ServiceProvider.GetRequiredService<GrulaDriverIdCache>(),
                second.ServiceProvider.GetRequiredService<GrulaDriverIdCache>());
        }

        [Fact]
        public void TheValidator_IsScopedSoItsMemoryNeverOutlivesARequest()
        {
            using var provider = BuildSellerProvider(CreateConfiguration(true));

            using var first = provider.CreateScope();
            using var second = provider.CreateScope();

            var inFirst = first.ServiceProvider.GetRequiredService<IDiscountCodeValidator>();

            Assert.Same(inFirst, first.ServiceProvider.GetRequiredService<IDiscountCodeValidator>());
            Assert.NotSame(inFirst, second.ServiceProvider.GetRequiredService<IDiscountCodeValidator>());
        }

        [Fact]
        public void EnforcementSetting_DefaultsToOffInBothApps()
        {
            Assert.False(new SellerAppSettings().DiscountCodeEnforcementEnabled);
            Assert.False(new BuyerAppSettings().DiscountCodeEnforcementEnabled);
        }

        [Fact]
        public void EnforcementSetting_BindsFromConfigurationAndAppliesToEverySeamTogether()
        {
            using var provider = BuildSellerProvider(CreateConfiguration(true));
            using var scope = provider.CreateScope();

            var settings = scope.ServiceProvider.GetRequiredService<IPricingSettings>();

            Assert.True(settings.DiscountCodeEnforcementEnabled);
            Assert.True(settings.IsDiscountCodeEnforced());
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, false)]
        public void IsDiscountCodeEnforced_NeedsBothGrulaAndTheSetting(bool grulaConfigured, bool enabled, bool expected)
        {
            var settings = new Helpers.TestPricingSettings
            {
                GrulaAccessToken = grulaConfigured ? "token" : null,
                DiscountCodeEnforcementEnabled = enabled
            };

            Assert.Equal(expected, settings.IsDiscountCodeEnforced());
        }

        [Fact]
        public void EnforcementSetting_ForAnUnconfiguredGrula_IsNeverEnforcedEvenWhenEnabled()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> { ["DiscountCodeEnforcementEnabled"] = "true", ["ClientUrl"] = "http://localhost:5001" })
                .Build();

            using var provider = BuildSellerProvider(configuration);
            using var scope = provider.CreateScope();

            Assert.False(scope.ServiceProvider.GetRequiredService<IPricingSettings>().IsDiscountCodeEnforced());
        }
    }
}
