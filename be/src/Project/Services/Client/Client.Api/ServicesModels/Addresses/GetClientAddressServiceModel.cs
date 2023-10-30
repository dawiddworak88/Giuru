using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Addresses
{
    public class GetClientAddressServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
