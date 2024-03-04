using System;

namespace Client.Api.ServicesModels.FieldValues
{
    public class CreateClientFieldValueServiceModel
    {
        public Guid? FieldDefinitionId { get; set; }
        public string FieldValue { get; set; }
    }
}
