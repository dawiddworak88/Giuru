using System;

namespace Client.Api.v1.RequestModels
{
    public class ClientRequestModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
    }
}
