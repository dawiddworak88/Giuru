using Foundation.Extensions.Models;
using System;

namespace Identity.Api.ServicesModels.Organisations
{
    public class UpdateOrganisationStatusServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public bool IsDisabled { get; set; }
    }
}
