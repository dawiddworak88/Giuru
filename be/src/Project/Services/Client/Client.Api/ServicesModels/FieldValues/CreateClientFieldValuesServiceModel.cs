using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Client.Api.ServicesModels.FieldValues
{
    public class CreateClientFieldValuesServiceModel : BaseServiceModel
    {
        public Guid? ClientId { get; set; }
        public IEnumerable<CreateClientFieldValueServiceModel> FieldsValues { get; set; }
    }
}
