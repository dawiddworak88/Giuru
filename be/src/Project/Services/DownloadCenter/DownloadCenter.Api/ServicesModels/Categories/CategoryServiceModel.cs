using System;
using System.Collections.Generic;

namespace DownloadCenter.Api.ServicesModels.Categories
{
    public class CategoryServiceModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }
        public bool IsVisible { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
