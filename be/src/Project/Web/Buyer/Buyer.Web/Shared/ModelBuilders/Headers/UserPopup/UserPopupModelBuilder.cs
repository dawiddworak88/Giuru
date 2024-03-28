using Buyer.Web.Shared.ViewModels.Headers.UserPopup;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Links.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.ModelBuilders.Headers.UserPopup
{
    public class UserPopupModelBuilder : IComponentModelBuilder<ComponentModelBase, UserPopupViewModel>
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;

        public UserPopupModelBuilder(
            LinkGenerator linkGenerator,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer)
        {
            _linkGenerator = linkGenerator;
            _globalLocalizer = globalLocalizer;
            _orderLocalizer = orderLocalizer;
        }

        public UserPopupViewModel BuildModel(ComponentModelBase componentModel)
        {
            var viewMode = new UserPopupViewModel
            {
                WelcomeText = _globalLocalizer.GetString("Welcome").Value,
                Name = componentModel.Name,
                IsLoggedIn = componentModel.IsAuthenticated,
                SignInLink = new LinkViewModel
                {
                    Url = _linkGenerator.GetPathByAction("Index", "Orders", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                    Text = _globalLocalizer.GetString("SignIn")
                },
                SignOutLink = new LinkViewModel
                {
                    Url = _linkGenerator.GetPathByAction("SignOutNow", "Account", new { Area = "Accounts", culture = CultureInfo.CurrentUICulture.Name }),
                    Text = _globalLocalizer.GetString("SignOut")
                },
                
            };

            var actions = new List<LinkViewModel>
            {
                new LinkViewModel
                {
                    Text = _orderLocalizer.GetString("MyOrders").Value,
                    Url = _linkGenerator.GetPathByAction("Index", "Orders", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name })
                },
                new LinkViewModel
                {
                    Text = _orderLocalizer.GetString("PlaceOrder").Value,
                    Url = _linkGenerator.GetPathByAction("Index", "Order", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name })
                }
            };

            viewMode.Actions = actions;

            return viewMode;
        }
    }
}
