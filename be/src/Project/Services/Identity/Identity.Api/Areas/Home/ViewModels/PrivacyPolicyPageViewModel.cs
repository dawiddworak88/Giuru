using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.Metadatas.ViewModels;

namespace Identity.Api.Areas.Home.ViewModels
{
    public class PrivacyPolicyPageViewModel
    {
        public MetadataViewModel Metadata { get; set; }
        public HeaderViewModel Header { get; set; }
        public PolicyPageViewModel Policy { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
