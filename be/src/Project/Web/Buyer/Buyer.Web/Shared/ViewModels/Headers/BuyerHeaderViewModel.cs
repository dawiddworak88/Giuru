using Buyer.Web.Shared.ViewModels.Headers.Search;
using Buyer.Web.Shared.ViewModels.Headers.SidebarMobile;
using Buyer.Web.Shared.ViewModels.Headers.UserPopup;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.Links.ViewModels;

namespace Buyer.Web.Shared.ViewModels.Headers
{
    public class BuyerHeaderViewModel : HeaderViewModel
    {
        public UserPopupViewModel UserPopup { get; set; }
        public SearchViewModel Search { get; set; }
        public SidebarMobileViewModel SidebarMobile { get; set; }
        public string BasketUrl { get; set; }
        public double TotalBasketItems { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string GoToCartLabel { get; set; }
    }
}
