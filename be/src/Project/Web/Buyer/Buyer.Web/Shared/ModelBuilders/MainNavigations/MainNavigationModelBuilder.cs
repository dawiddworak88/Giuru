using Foundation.Extensions.ModelBuilders;
using System.Collections.Generic;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.Links.ViewModels;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using System.Globalization;
using Microsoft.Extensions.Options;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Repositories.Brands;

namespace Buyer.Web.Shared.ModelBuilders.MainNavigations
{
    public class MainNavigationModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IOptions<AppSettings> configuration;
        private readonly IBrandRepository brandRepository;

        public MainNavigationModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            LinkGenerator linkGenerator,
            IOptions<AppSettings> configuration,
            IBrandRepository brandRepository)
        {
            this.globalLocalizer = globalLocalizer;
            this.orderLocalizer = orderLocalizer;
            this.linkGenerator = linkGenerator;
            this.configuration = configuration;
            this.brandRepository = brandRepository;
        }

        public async Task<MainNavigationViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var links = new List<LinkViewModel>
            { 
                new LinkViewModel 
                {
                    Text = this.globalLocalizer.GetString("Home"),
                    Url = this.linkGenerator.GetPathByAction("Index", "Home", new { Area = "Home", culture = CultureInfo.CurrentUICulture.Name })
                }
            };

            if (componentModel.IsAuthenticated)
            {
                links.Add(new LinkViewModel
                {
                    Text = this.orderLocalizer.GetString("Orders").Value,
                    Url = this.linkGenerator.GetPathByAction("Index", "Orders", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name })
                });

                links.Add(new LinkViewModel
                {
                    Text = this.orderLocalizer.GetString("NewOrder").Value,
                    Url = this.linkGenerator.GetPathByAction("Index", "Order", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name })
                });
            }

            links.Add(new LinkViewModel
            {
                Text = this.globalLocalizer.GetString("AvailableProducts"),
                Url = this.linkGenerator.GetPathByAction("Index", "AvailableProducts", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
            });

            if (!this.configuration.Value.IsMarketplace)
            {
                var brand = await this.brandRepository.GetBrandAsync(this.configuration.Value.OrganisationId, componentModel.Token, componentModel.Language);

                var brandZoneLink = new LinkViewModel
                { 
                    Text = this.globalLocalizer.GetString("Catalog").Value,
                    Url = this.linkGenerator.GetPathByAction("Index", "Brand", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = this.configuration.Value.OrganisationId })
                };

                links.Add(brandZoneLink);
            }

            return new MainNavigationViewModel
            {
                Links = links
            };
        }
    }
}
