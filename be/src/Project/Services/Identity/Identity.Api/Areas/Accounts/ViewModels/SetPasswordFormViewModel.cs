using System;
using System.Collections.Generic;

namespace Identity.Api.Areas.Accounts.ViewModels
{
    public class SetPasswordFormViewModel
    {
        public Guid? Id { get; set; }
        public string ReturnUrl { get; set; }
        public string SubmitUrl { get; set; }
        public string SetPasswordText { get; set; }
        public string PasswordLabel { get; set; }
        public string ConfirmPasswordLabel { get; set; }
        public string EmailIsConfirmedText { get; set; }
        public string BackToLoginText { get; set; }
        public string PasswordFormatErrorMessage { get; set; }
        public string PasswordRequiredErrorMessage { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string PasswordSetSuccessMessage { get; set; }
        public string MarketingApprovalHeader { get; set; }
        public string MarketingApprovalText { get; set; }
        public string PersonalDataAdministratorText { get; set; }
        public IEnumerable<ApprovalViewModel> Approvals { get; set; }
    }
}
