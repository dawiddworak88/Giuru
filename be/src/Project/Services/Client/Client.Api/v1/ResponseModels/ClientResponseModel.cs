using System;
using System.Collections.Generic;

namespace Client.Api.v1.ResponseModels
{
    public class ClientResponseModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CommunicationLanguage { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<Guid> Groups { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
