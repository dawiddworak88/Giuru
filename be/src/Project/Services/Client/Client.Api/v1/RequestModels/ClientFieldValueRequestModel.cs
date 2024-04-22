using System;

namespace Client.Api.v1.RequestModels
{
    public class ClientFieldValueRequestModel
    {
        public Guid? FieldDefinitionId { get; set; }
        public string FieldValue { get; set; }
    }
}
