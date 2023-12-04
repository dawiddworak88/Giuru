using Foundation.Extensions.Models;
using System.Collections.Generic;

namespace Client.Api.ServicesModels.Fields
{
    public class CreateClientFieldServiceModel : BaseServiceModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsRequired { get; set; }
        public IEnumerable<ClientFieldOptionServiceModel> Options { get; set; }
    }
}
