namespace Identity.Api.Areas.Accounts.ComponentModels
{
    public class SignInFormComponentModel
    {
        public string ReturnUrl { get; set; }
        public string DevelopersEmail { get; set; }
        public string ErrorMessage { get; set; }
    }
}
