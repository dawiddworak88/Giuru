using Foundation.Extensions.Models;
using System;

namespace Media.Api.ServicesModels
{
    public class UpdateMediaItemVersionServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaData { get; set; }
    }
}
