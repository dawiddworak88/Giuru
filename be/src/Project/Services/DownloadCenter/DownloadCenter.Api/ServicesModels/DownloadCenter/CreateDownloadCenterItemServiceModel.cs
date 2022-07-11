using Foundation.Extensions.Models;
using System;

namespace DownloadCenter.Api.ServicesModels.DownloadCenter
{
    public class CreateDownloadCenterItemServiceModel : BaseServiceModel
    {
        public Guid? CategoryId { get; set; }
        public int? Order { get; set; }
    }
}
