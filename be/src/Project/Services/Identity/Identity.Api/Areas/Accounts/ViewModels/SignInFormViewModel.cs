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
        public string ForgotPasswordLabel { get; set; }
        public string ResetPasswordUrl { get; set; }
        public string RegisterLabel { get; set; }
        public string RegisterButtonText { get; set; }
        public string ContactText{ get; set; }
        public string RegisterUrl { get; set; }
        public string DevelopersEmail { get; set; }
        public string ErrorMessage { get; set; }
    }
}
