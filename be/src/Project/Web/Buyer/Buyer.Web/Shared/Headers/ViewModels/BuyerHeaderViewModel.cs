using Foundation.PageContent.Components.Headers.ViewModels;

namespace Buyer.Web.Shared.Headers.ViewModels
{
    public class BuyerHeaderViewModel : HeaderViewModel
    {
        public string SearchUrl { get; set; }
        public string SearchTerm { get; set; }
        public string SearchLabel { get; set; }
        public string SearchPlaceholderLabel { get; set; }
        public string GetSuggestionsUrl { get; set; }
        public string GeneralErrorMessage { get; set; }
    }
}
