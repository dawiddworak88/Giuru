﻿using Foundation.Extensions.Models;
using System;

namespace DownloadCenter.Api.ServicesModels.Categories
{
    public class DeleteCategoryServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
