using System.Collections.Generic;

namespace Identity.Api.v1.RequestModels
{
    public class RolesRequestModel
    {
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
