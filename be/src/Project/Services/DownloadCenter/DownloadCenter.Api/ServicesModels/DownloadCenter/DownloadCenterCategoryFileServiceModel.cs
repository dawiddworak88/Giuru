using Foundation.Extensions.Models;
using System;

namespace DownloadCenter.Api.ServicesModels.DownloadCenter
{
    public class DownloadCenterCategoryFileServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Filename { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
