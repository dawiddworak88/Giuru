using Foundation.Extensions.Models;
using System.Collections.Generic;

namespace Identity.Api.ServicesModels.Roles
{
    public class CreateRolesServiceModel : BaseServiceModel
    {
        public IEnumerable<string> Roles { get; set; }
    }
}
