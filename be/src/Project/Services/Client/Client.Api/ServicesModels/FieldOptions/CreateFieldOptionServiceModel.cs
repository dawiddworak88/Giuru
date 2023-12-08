using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.FieldOptions
{
    public class CreateFieldOptionServiceModel : BaseServiceModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public Guid? FieldDefinitionId { get; set; }
    }
}
