using Foundation.Extensions.Models;
using System.Collections.Generic;
using System;

namespace Client.Api.ServicesModels.Addresses
{
    public class GetClientAddressesByIdsServiceModel : PagedBaseServiceModel
    {
        public IEnumerable<Guid> Ids { get; set; }
    }
}
