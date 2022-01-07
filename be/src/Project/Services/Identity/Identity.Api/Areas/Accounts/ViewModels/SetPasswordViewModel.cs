using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;

namespace Identity.Api.Areas.Accounts.ViewModels
{
    public class SetPasswordViewModel
    {
        public HeaderViewModel Header { get; set; }
        public SetPasswordFormViewModel SetPasswordForm { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
