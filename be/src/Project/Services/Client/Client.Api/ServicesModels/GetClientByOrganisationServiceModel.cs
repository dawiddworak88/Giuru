using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels
{
    public class GetClientByOrganisationServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
