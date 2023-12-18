using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.FieldValues
{
    public class GetClientFieldValuesServiceModel : PagedBaseServiceModel
    {
        public Guid? ClientId { get; set; }
    }
}
