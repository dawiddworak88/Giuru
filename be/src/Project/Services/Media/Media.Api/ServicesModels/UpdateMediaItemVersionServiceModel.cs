using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Media.Api.ServicesModels
{
    public class UpdateMediaItemVersionServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaData { get; set; }
        public IEnumerable<Guid> ClientGroupIds { get; set; }
    }
}
