﻿using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace DownloadCenter.Api.ServicesModels.DownloadCenter
{
    public class CreateDownloadCenterItemServiceModel : BaseServiceModel
    {
        public IEnumerable<Guid> CategoriesIds { get; set; }
        public IEnumerable<DownloadCenterFileServiceModel> Files { get; set; }
    }
}
