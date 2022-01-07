namespace Identity.Api.Areas.Accounts.ApiRequestModels
{
    public class SaveUserRequestModel
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
