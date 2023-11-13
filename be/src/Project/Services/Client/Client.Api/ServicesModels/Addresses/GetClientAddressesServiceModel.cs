using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Addresses
{
    public class GetClientAddressesServiceModel : PagedBaseServiceModel
    {
        public Guid? ClientId { get; set; }
    }
}
