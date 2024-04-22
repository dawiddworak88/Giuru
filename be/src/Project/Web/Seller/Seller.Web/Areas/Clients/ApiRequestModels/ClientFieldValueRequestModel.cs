using System;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    public class ClientFieldValueRequestModel
    {
        public string FieldValue { get; set; }
        public Guid? FieldDefinitionId { get; set; }
    }
}
