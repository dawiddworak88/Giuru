using Foundation.Extensions.Models;
using System;

namespace Catalog.Api.v1.Areas.Clients.Models.GetClient
{
    public class GetClientModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
