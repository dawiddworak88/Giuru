namespace Identity.Api.Areas.Accounts.ViewModels
{
    public class ResetPasswordFormViewModel
    {
        public string ResetPasswordText { get; set; }
        public string EmailRequiredErrorMessage { get; set; }
        public string EmailFormatErrorMessage { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string SubmitUrl { get; set; }
        public string EmailLabel { get; set; }
    }
}
