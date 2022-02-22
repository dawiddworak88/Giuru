using Foundation.ApiExtensions.Models.Request;

namespace Media.Api.v1.RequestModels
{
    public class UpdateMediaItemVersionRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaData { get; set; }
    }
}
