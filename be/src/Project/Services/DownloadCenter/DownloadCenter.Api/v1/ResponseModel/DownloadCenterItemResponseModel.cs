using System;
using System.Collections.Generic;

namespace DownloadCenter.Api.v1.ResponseModel
{
    public class DownloadCenterItemResponseModel
    {
        public Guid? Id { get; set; }
        public string Filename { get; set; }
        public IEnumerable<Guid> ClientGroupIds { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
