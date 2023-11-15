using Buyer.Web.Shared.ViewModels.Base;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.Metadatas.ViewModels;

namespace Buyer.Web.Areas.Orders.ViewModel
{
    public class OrderItemPageViewModel : BasePageViewModel
    {
        public MetadataViewModel Metadata { get; set; }
        public MainNavigationViewModel MainNavigation { get; set; }
        public OrderItemFormViewModel OrderItemForm { get; set; }
    }
}
