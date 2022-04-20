using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Client.Api.ServicesModels.Groups
{
    public class GetGroupsByIdsServiceModel : PagedBaseServiceModel
    {
        public IEnumerable<Guid> Ids { get; set; }
    }
}
