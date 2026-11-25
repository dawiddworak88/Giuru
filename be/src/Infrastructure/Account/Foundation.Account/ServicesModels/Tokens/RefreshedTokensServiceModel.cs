namespace Foundation.Account.ServicesModels.Tokens
{
    public class RefreshedTokensServiceModel
    {
        public bool Succeeded { get; set; }
        public bool IsRefreshTokenRejected { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string IdentityToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
