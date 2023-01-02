using Buyer.Web.Shared.Definitions.DashboardNavigation;
using Buyer.Web.Shared.ViewModels.DashboardNavigation;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Globalization;

namespace Buyer.Web.Shared.ModelBuilders.DashboardNavigation
{
    public class DashboardNavigationModelBuilder : IModelBuilder<DashboardNavigationViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly LinkGenerator linkGenerator;

        public DashboardNavigationModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public DashboardNavigationViewModel BuildModel()
        {
            return new DashboardNavigationViewModel
            {
                NavigationItems = new List<DashboardNavigationItemViewModel>
                {
                    new DashboardNavigationItemViewModel
                    {
                        Title = this.globalLocalizer.GetString("Settings"),
                        Icon = DashboardNavigationConstants.SettingsIcon,
                        Url = this.linkGenerator.GetPathByAction("Index", "Settings", new { Area = "Dashboard", culture = CultureInfo.CurrentUICulture.Name })
                    }
                }
            };
        }
    }
}
