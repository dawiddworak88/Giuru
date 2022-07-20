using System;
using System.Collections.Generic;

namespace DownloadCenter.Api.v1.ResponseModel
{
    public class DownloadCenterFileCategoriesResponseModel
    {
        public Guid? Id { get; set; }
        public IEnumerable<Guid> CategoriesIds { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
