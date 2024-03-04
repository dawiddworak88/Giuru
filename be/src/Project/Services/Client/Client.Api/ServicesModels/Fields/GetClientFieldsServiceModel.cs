using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Fields
{
    public class GetClientFieldsServiceModel : PagedBaseServiceModel
    {
        public Guid? ClientId { get; set; }
    }
}
