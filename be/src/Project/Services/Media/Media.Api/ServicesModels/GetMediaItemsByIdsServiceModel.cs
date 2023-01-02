using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Media.Api.ServicesModels
{
    public class GetMediaItemsByIdsServiceModel : BaseServiceModel
    {
        public IEnumerable<Guid> Ids { get; set; }
        public string SearchTerm { get; set; }
        public int? PageIndex { get; set; }
        public int? ItemsPerPage { get; set; }
    }
}
