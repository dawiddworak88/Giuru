using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;

namespace Identity.Api.Areas.Accounts.ViewModels
{
    public class ResetPasswordViewModel
    {
        public HeaderViewModel Header { get; set; }
        public ResetPasswordFormViewModel ResetPasswordForm { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
