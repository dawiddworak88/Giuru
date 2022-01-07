using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;

namespace Identity.Api.Areas.Home.ViewModels
{
    public class PrivacyPolicyPageViewModel
    {
        public HeaderViewModel Header { get; set; }
        public ContentPageViewModel Content { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
