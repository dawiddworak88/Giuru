using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.FieldOptions
{
    public class UpdateClientFieldOptionServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public Guid? FieldDefinitionId { get; set; }
    }
}
