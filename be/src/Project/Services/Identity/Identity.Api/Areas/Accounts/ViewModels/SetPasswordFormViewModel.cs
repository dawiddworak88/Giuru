using System;

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
        public bool EmailConfirmed { get; set; }
        public string PasswordFormatErrorMessage { get; set; }
        public string PasswordRequiredErrorMessage { get; set; }
        public string FirstNameRequiredErrorMessage { get; set; }
        public string LastNameRequiredErrorMessage { get; set; }
    }
}
