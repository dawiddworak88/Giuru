using System;
using System.Collections.Generic;

namespace DownloadCenter.Api.ServicesModels.DownloadCenter
{
    public class DownloadCenterItemServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<DownloadCenterSubcategoryServiceModel> Categories { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
