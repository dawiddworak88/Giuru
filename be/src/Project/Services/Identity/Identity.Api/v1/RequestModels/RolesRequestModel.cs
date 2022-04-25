using System.Collections.Generic;

namespace Identity.Api.v1.RequestModels
{
    public class RolesRequestModel
    {
        public IEnumerable<string> Roles { get; set; }
    }
}
