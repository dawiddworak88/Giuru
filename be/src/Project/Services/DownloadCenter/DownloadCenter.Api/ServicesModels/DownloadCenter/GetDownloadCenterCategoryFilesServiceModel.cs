using Foundation.Extensions.Models;
using System;

namespace DownloadCenter.Api.ServicesModels.DownloadCenter
{
    public class GetDownloadCenterCategoryFilesServiceModel : PagedBaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
