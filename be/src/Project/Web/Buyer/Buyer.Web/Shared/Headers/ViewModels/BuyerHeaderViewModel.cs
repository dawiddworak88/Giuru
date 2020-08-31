using Foundation.PageContent.Components.Headers.ViewModels;

namespace Buyer.Web.Shared.Headers.ViewModels
{
    public class BuyerHeaderViewModel : HeaderViewModel
    {
        public string SearchUrl { get; set; }
        public string SearchLabel { get; set; }
        public string SearchPlaceholderLabel { get; set; }
    }
}
