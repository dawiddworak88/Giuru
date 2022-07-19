using Foundation.Extensions.Models;
using System;

namespace DownloadCenter.Api.ServicesModels.DownloadCenter
{
    public class DeleteDownloadCenterFileServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
