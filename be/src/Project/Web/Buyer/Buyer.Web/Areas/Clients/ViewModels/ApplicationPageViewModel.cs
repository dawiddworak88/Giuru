using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;

namespace Buyer.Web.Areas.Clients.ViewModels
{
    public class ApplicationPageViewModel
    {
        public HeaderViewModel Header { get; set; }
        public ApplicationFormViewModel ApplicationForm { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
