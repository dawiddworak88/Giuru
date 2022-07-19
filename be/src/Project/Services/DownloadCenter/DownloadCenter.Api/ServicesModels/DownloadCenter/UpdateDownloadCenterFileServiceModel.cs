using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace DownloadCenter.Api.ServicesModels.DownloadCenter
{
    public class UpdateDownloadCenterFileServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public IEnumerable<Guid> CategoriesIds { get; set; }
        public IEnumerable<Guid> Files { get; set; }
    }
}
