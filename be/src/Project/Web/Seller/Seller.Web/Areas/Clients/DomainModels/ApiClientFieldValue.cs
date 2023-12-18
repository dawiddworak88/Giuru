using System;

namespace Seller.Web.Areas.Clients.DomainModels
{
    public class ApiClientFieldValue
    {
        public string FieldValue { get; set; }
        public Guid? FieldDefinitionId { get; set; }
    }
}
