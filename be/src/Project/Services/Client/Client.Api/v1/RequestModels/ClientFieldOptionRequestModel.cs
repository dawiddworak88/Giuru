using System;

namespace Client.Api.v1.RequestModels
{
    public class ClientFieldOptionRequestModel
    {
        public Guid? Id {  get; set; }
        public string Name { get; set; }
        public Guid? FieldDefinitionId { get; set; }
    }
}
