using Media.Api.v1.Areas.Media.ResultModels;
using System;
using System.Collections.Generic;

namespace Media.Api.ServicesModels
{
    public class MediaItemVerionsByIdServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<MediaItemServiceModel> Versions { get; set; }
    }
}
