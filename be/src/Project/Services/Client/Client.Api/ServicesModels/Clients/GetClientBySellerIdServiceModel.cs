using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Clients
{
    public class GetClientBySellerIdServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
