namespace Identity.Api.v1.RequestModels
{
    public class ResetUserPasswordRequestModel
    {
        public string Email { get; set; }
        public string ReturnUrl { get; set; }
    }
}
