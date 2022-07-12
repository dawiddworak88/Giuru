using Buyer.Web.Shared.ViewModels.Base;
using Foundation.PageContent.Components.MainNavigations.ViewModels;

namespace Buyer.Web.Areas.DownloadCenter.ViewModel
{
    public class CategoryPageViewModel : BasePageViewModel
    {
        public MainNavigationViewModel MainNavigation { get; set; }
        public CategoryDetailsViewModel CategoryDetails { get; set; }
    }
}
