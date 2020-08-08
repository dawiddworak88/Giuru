namespace Identity.Api.Areas.Accounts.Models
{
    public class SignInModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
