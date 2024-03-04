using System;

namespace Client.Api.v1.ResponseModels
{
    public class ClientFieldValueResponseModel
    {
        public Guid? Id { get; set; }
        public string FieldValue { get; set; }
        public Guid? FieldDefinitionId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
