using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.FieldOptions
{
    public class GetClientFieldOptionsServiceModel : PagedBaseServiceModel
    {
        public Guid? FieldDefinitionId { get; set; }
    }
}
