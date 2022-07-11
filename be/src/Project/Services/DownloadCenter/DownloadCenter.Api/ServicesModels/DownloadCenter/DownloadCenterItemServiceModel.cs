using System;

namespace DownloadCenter.Api.ServicesModels.DownloadCenter
{
    public class DownloadCenterItemServiceModel
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? Order { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
