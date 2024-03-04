using System;
using System.Collections.Generic;

namespace Client.Api.v1.RequestModels
{
    public class ClientFieldValuesRequestModel
    {
        public Guid? ClientId { get; set; }
        public IEnumerable<ClientFieldValueRequestModel> FieldsValues { get; set; }
    }
}
