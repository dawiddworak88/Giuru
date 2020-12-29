using Foundation.Extensions.Models;
using System;

namespace Identity.Api.v1.Areas.Clients.Models
{
    public class GetClientModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
