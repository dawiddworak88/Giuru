using System.Collections.Generic;

namespace Seller.Web.Shared.ApiRequestModels
{
    public class RolesRequestModel
    {
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
