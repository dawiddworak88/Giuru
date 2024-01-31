using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.FieldOptions
{
    public class CreateClientFieldOptionServiceModel : BaseServiceModel
    {
        public string Name { get; set; }
        public Guid? FieldDefinitionId { get; set; }
    }
}
