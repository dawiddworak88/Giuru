using Foundation.Extensions.Models;

namespace Identity.Api.ServicesModels.Users
{
    public class GetUserByEmailServiceModel : BaseServiceModel
    {
        public string Email { get; set; }
    }
}
