using System;

namespace Client.Api.ServicesModels.FieldValues
{
    public class ClientFieldValueServiceModel
    {
        public Guid? Id { get; set; }
        public string FieldValue { get; set; }
        public Guid? FieldDefinitionId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
