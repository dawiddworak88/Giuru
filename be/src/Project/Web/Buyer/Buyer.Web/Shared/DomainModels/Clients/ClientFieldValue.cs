using System;

namespace Buyer.Web.Shared.DomainModels.Clients
{
    public class ClientFieldValue
    {
        public Guid Id { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public Guid FieldDefinitionId { get; set; }
    }
}
