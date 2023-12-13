using System;

namespace Client.Api.v1.ResponseModels
{
    public class ClientFieldOptionResponseModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public Guid? FieldDefinitionId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
