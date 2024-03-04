using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Fields
{
    public class UpdateClientFieldServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsRequired { get; set; }
    }
}
