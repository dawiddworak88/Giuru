using Foundation.Extensions.Models;
using System;

namespace Identity.Api.ServicesModels.Organisations
{
    public class UpdateOrganisationServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
    }
}
