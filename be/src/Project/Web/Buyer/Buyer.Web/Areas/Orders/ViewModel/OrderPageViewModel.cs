using Buyer.Web.Shared.ViewModels.Base;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.Metadatas.ViewModels;

namespace Buyer.Web.Areas.Orders.ViewModel
{
    public class OrderPageViewModel : BasePageViewModel
    {
        public MetadataViewModel Metadata { get; set; }
        public MainNavigationViewModel MainNavigation { get; set; }
        public OrderFormViewModel NewOrderForm { get; set; }
    }
}
