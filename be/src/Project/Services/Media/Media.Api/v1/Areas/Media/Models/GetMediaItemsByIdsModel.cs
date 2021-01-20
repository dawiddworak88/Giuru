using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Media.Api.v1.Areas.Media.Models
{
    public class GetMediaItemsByIdsModel : BaseServiceModel
    {
        public IEnumerable<Guid> Ids { get; set; }
        public int PageIndex { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
