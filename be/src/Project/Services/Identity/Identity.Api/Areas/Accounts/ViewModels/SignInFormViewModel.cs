namespace Identity.Api.ViewModels.SignInForm
{
    public class SignInFormViewModel
    {
        public string ReturnUrl { get; set; }
        public string SubmitUrl { get; set; }
        public string EmailRequiredErrorMessage { get; set; }
        public string PasswordRequiredErrorMessage { get; set; }
        public string EmailFormatErrorMessage { get; set; }
        public string PasswordFormatErrorMessage { get; set; }
        public string SignInText { get; set; }
        public string EnterEmailText { get; set; }
        public string EnterPasswordText { get; set; }
    }
}
