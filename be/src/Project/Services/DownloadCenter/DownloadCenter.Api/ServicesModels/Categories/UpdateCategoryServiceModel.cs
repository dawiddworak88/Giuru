using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace DownloadCenter.Api.ServicesModels.Categories
{
    public class UpdateCategoryServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string Name { get; set; }
        public bool IsVisible { get; set; }
    }
}
