using Foundation.ApiExtensions.Models.Request;
using System.Collections.Generic;

namespace Client.Api.v1.RequestModels
{
    public class ClientFieldRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsRequired { get; set; }
        public IEnumerable<ClientFieldOptionRequestModel> Options { get; set; }
    }
}
