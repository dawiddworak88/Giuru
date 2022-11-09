using Foundation.Extensions.ModelBuilders;
using System.Collections.Generic;
using Foundation.PageContent.Components.Links.ViewModels;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using Foundation.PageContent.Components.LanguageSwitchers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Buyer.Web.Shared.ViewModels.Headers;
using System.Globalization;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Foundation.Extensions.ExtensionMethods;
using System.Linq;

namespace Buyer.Web.Shared.ModelBuilders.Headers
{
    public class BuyerHeaderModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel>
    {
        private readonly IModelBuilder<LogoViewModel> logoModelBuilder;
        private readonly IModelBuilder<LanguageSwitcherViewModel> languageSwitcherViewModel;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IBasketRepository basketRepository;

        public BuyerHeaderModelBuilder(
            IModelBuilder<LogoViewModel> logoModelBuilder,
            IModelBuilder<LanguageSwitcherViewModel> languageSwitcherViewModel,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IBasketRepository basketRepository,
            LinkGenerator linkGenerator)
        {
            this.logoModelBuilder = logoModelBuilder;
            this.languageSwitcherViewModel = languageSwitcherViewModel;
            this.globalLocalizer = globalLocalizer;
            this.linkGenerator = linkGenerator;
            this.basketRepository = basketRepository;
        }

        public async Task<BuyerHeaderViewModel> BuildModelAsync(ComponentModelBase model)
        {
            var links = new List<LinkViewModel>
            {
                new LinkViewModel { Text = this.globalLocalizer["PriceList"], Url = "#price-list" },
                new LinkViewModel { Text = this.globalLocalizer["Contact"], Url = "#contact" }
            };

            var viewModel = new BuyerHeaderViewModel
            {
                WelcomeText = this.globalLocalizer.GetString("Welcome").Value,
                GoToCartLabel = this.globalLocalizer.GetString("Basket"),
                Name = model.Name,
                IsLoggedIn = model.IsAuthenticated,
                SearchUrl = this.linkGenerator.GetPathByAction("Index", "SearchProducts", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                SearchLabel = this.globalLocalizer.GetString("Search"),
                SearchPlaceholderLabel = this.globalLocalizer.GetString("Search"),
                Logo = this.logoModelBuilder.BuildModel(),
                BasketUrl = this.linkGenerator.GetPathByAction("Index", "Basket", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                LanguageSwitcher = this.languageSwitcherViewModel.BuildModel(),
                GetSuggestionsUrl = this.linkGenerator.GetPathByAction("Get", "SearchSuggestionsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                SearchTerm = string.Empty,
                SignInLink = new LinkViewModel
                {
                    Url = this.linkGenerator.GetPathByAction("Index", "Orders", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                    Text = this.globalLocalizer.GetString("SignIn")
                },
                SignOutLink = new LinkViewModel
                {
                    Url = this.linkGenerator.GetPathByAction("SignOutNow", "Account", new { Area = "Accounts", culture = CultureInfo.CurrentUICulture.Name }),
                    Text = this.globalLocalizer.GetString("SignOut")
                },
                Links = links
            };

            if (model.IsAuthenticated && model.BasketId.HasValue)
            {
                var existingBasket = await this.basketRepository.GetBasketById(model.Token, model.Language, model.BasketId);
                if (existingBasket is not null)
                {
                    var basketItems = existingBasket.Items.OrEmptyIfNull();
                    if (basketItems.Any())
                    {
                        double sum = 0;
                        foreach (var item in basketItems)
                        {
                            sum += item.StockQuantity + item.OutletQuantity + item.Quantity;
                        }
       
                        viewModel.TotalBasketItems = sum;
                    }
                }
            }

            return viewModel;
        }
    }
}
