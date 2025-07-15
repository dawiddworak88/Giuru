using System;

namespace Seller.Web.Areas.Clients.DomainModels
{
    public class ClientFieldValue
    {
        public Guid Id { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public Guid FieldDefinitionId { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
