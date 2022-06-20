using Media.Api.v1.Areas.Media.ResultModels;
using System;
using System.Collections.Generic;

namespace Media.Api.v1.ResponseModels
{
    public class MediaItemVersionsResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaData { get; set; }
        public IEnumerable<MediaItemServiceModel> Versions { get; set; }
    }
}
