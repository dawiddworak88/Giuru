using Foundation.Extensions.Models;
using System;

namespace DownloadCenter.Api.ServicesModels.DownloadCenter
{
    public class UpdateDownloadCenterItemServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public Guid? CategoryId { get; set; }
        public int? Order { get; set; }
    }
}
