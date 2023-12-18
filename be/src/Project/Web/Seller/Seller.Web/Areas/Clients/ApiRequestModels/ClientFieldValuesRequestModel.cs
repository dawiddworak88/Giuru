using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    public class ClientFieldValuesRequestModel
    {
        public Guid? ClientId { get; set; }
        public IEnumerable<ClientFieldValueRequestModel> FieldsValues { get; set; }
    }
}
