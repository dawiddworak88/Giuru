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
using Buyer.Web.Shared.ViewModels.Headers.Search;
using Buyer.Web.Shared.ViewModels.Headers.SidebarMobile;
using Buyer.Web.Shared.ViewModels.Headers.UserPopup;

namespace Buyer.Web.Shared.ModelBuilders.Headers
{
    public class BuyerHeaderModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel>
    {
        private readonly IModelBuilder<LogoViewModel> _logoModelBuilder;
        private readonly IModelBuilder<SearchViewModel> _searchModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SidebarMobileViewModel> _sidebarMobileModelBuilder;
        private readonly IComponentModelBuilder<ComponentModelBase, UserPopupViewModel> _userPopupModelBuilder;
        private readonly IModelBuilder<LanguageSwitcherViewModel> _languageSwitcherViewModel;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly LinkGenerator _linkGenerator;
        private readonly IBasketRepository _basketRepository;

        public BuyerHeaderModelBuilder(
            IModelBuilder<LogoViewModel> logoModelBuilder,
            IModelBuilder<SearchViewModel> searchModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, SidebarMobileViewModel> sidebarMobileModelBuilder,
            IComponentModelBuilder<ComponentModelBase, UserPopupViewModel> userPopupModelBuilder,
            IModelBuilder<LanguageSwitcherViewModel> languageSwitcherViewModel,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IBasketRepository basketRepository,
            LinkGenerator linkGenerator)
        {
            _logoModelBuilder = logoModelBuilder;
            _searchModelBuilder = searchModelBuilder;
            _sidebarMobileModelBuilder = sidebarMobileModelBuilder;
            _userPopupModelBuilder = userPopupModelBuilder;
            _languageSwitcherViewModel = languageSwitcherViewModel;
            _globalLocalizer = globalLocalizer;
            _linkGenerator = linkGenerator;
            _basketRepository = basketRepository;
        }

        public async Task<BuyerHeaderViewModel> BuildModelAsync(ComponentModelBase model)
        {
            var links = new List<LinkViewModel>
            {
                new LinkViewModel { Text = _globalLocalizer["PriceList"], Url = "#price-list" },
                new LinkViewModel { Text = _globalLocalizer["Contact"], Url = "#contact" }
            };

            var viewModel = new BuyerHeaderViewModel
            {
                Search = _searchModelBuilder.BuildModel(),
                LanguageSwitcher = _languageSwitcherViewModel.BuildModel(),
                UserPopup = _userPopupModelBuilder.BuildModel(model),
                SidebarMobile = await _sidebarMobileModelBuilder.BuildModelAsync(model),
                GoToCartLabel = _globalLocalizer.GetString("Basket"),
                Logo = _logoModelBuilder.BuildModel(),
                BasketUrl = _linkGenerator.GetPathByAction("Index", "Order", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                Links = links
            };

            if (model.IsAuthenticated && model.BasketId.HasValue)
            {
                var existingBasket = await _basketRepository.GetBasketById(model.Token, model.Language, model.BasketId);
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
