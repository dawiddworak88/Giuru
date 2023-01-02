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

namespace Buyer.Web.Shared.ModelBuilders.MainNavigations
{
    public class MainNavigationModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IOptionsMonitor<AppSettings> settings;

        public MainNavigationModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            LinkGenerator linkGenerator,
            IOptionsMonitor<AppSettings> settings)
        {
            this.globalLocalizer = globalLocalizer;
            this.orderLocalizer = orderLocalizer;
            this.linkGenerator = linkGenerator;
            this.settings = settings;
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

            links.Add(new LinkViewModel
            {
                Text = this.orderLocalizer.GetString("MyOrders").Value,
                Url = this.linkGenerator.GetPathByAction("Index", "Orders", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name })
            });

            links.Add(new LinkViewModel
            {
                Text = this.orderLocalizer.GetString("PlaceOrder").Value,
                Url = this.linkGenerator.GetPathByAction("Index", "Order", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name })
            });

            links.Add(new LinkViewModel
            {
                Text = this.globalLocalizer.GetString("AvailableProducts"),
                Url = this.linkGenerator.GetPathByAction("Index", "AvailableProducts", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
            });

            links.Add(new LinkViewModel
            {
                Text = this.globalLocalizer.GetString("Outlet").Value,
                Url = this.linkGenerator.GetPathByAction("Index", "Outlet", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
            });

            links.Add(new LinkViewModel
            {
                Text = this.globalLocalizer.GetString("News"),
                Url = this.linkGenerator.GetPathByAction("Index", "News", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name })
            });

            links.Add(new LinkViewModel
            {
                Text = this.globalLocalizer.GetString("DownloadCenter"),
                Url = this.linkGenerator.GetPathByAction("Index", "DownloadCenter", new { Area = "DownloadCenter", culture = CultureInfo.CurrentUICulture.Name })
            });

            links.Add(new LinkViewModel
            {
                Text = this.globalLocalizer.GetString("MakeComplaint"),
                Target = "_blank",
                Url = this.settings.CurrentValue.MakeComplaintUrl
            });

            return new MainNavigationViewModel
            {
                Links = links
            };
        }
    }
}
