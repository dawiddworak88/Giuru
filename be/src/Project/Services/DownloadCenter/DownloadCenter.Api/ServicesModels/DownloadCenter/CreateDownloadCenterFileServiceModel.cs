using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace DownloadCenter.Api.ServicesModels.DownloadCenter
{
    public class CreateDownloadCenterFileServiceModel : BaseServiceModel
    {
        public IEnumerable<Guid> CategoriesIds { get; set; }
        public IEnumerable<DownloadCenterFilesServiceModel> Files { get; set; }
    }
}
