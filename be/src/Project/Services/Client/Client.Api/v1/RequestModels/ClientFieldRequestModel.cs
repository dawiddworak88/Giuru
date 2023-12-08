using Foundation.ApiExtensions.Models.Request;

namespace Client.Api.v1.RequestModels
{
    public class ClientFieldRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsRequired { get; set; }
    }
}
