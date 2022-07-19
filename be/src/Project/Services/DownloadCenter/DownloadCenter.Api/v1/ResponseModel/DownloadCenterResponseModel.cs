using System;
using System.Collections.Generic;

namespace DownloadCenter.Api.v1.ResponseModel
{
    public class DownloadCenterResponseModel
    {
        public Guid? Id { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
