using Buyer.Web.Shared.ViewModels.Base;
using Foundation.PageContent.Components.MainNavigations.ViewModels;

namespace Buyer.Web.Areas.Orders.ViewModel
{
    public class StatusOrderPageViewModel : BasePageViewModel
    {
        public MainNavigationViewModel MainNavigation { get; set; }
        public StatusOrderFormViewModel StatusOrder { get; set; }
    }
}
