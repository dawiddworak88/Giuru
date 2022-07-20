using System;
using System.Collections.Generic;

namespace DownloadCenter.Api.ServicesModels.DownloadCenter
{
    public class DownloadCenterFileServiceModel
    {
        public Guid? Id { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
