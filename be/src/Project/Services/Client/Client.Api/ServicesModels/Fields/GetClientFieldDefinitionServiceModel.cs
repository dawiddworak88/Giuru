using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Fields
{
    public class GetClientFieldDefinitionServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
