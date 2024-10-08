﻿using Foundation.Extensions.Models;
using System;

namespace News.Api.ServicesModels.Categories
{
    public class UpdateCategoryServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string Name { get; set; }
    }
}
