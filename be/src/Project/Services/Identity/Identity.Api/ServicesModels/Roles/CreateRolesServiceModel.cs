using Foundation.Extensions.Models;
using System.Collections.Generic;

namespace Identity.Api.ServicesModels.Roles
{
    public class CreateRolesServiceModel : BaseServiceModel
    {
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
