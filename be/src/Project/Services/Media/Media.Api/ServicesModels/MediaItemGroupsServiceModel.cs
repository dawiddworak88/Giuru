using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Media.Api.ServicesModels
{
    public class MediaItemGroupsServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public IEnumerable<Guid> GroupIds { get; set; }
    }
}
