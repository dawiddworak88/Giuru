using Seller.Web.Shared.ViewModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Products.ViewModels
{
    public class CategoryDetailFormViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public IEnumerable<FileViewModel> Files { get; set; }
        public string Title { get; set; }
        public string NameRequiredErrorMessage { get; set; }
        public string SelectCategoryLabel { get; set; }
        public string ParentCategoryLabel { get; set; }
        public string NameLabel { get; set; }
        public string SaveText { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string DropOrSelectFilesLabel { get; set; }
        public string DropFilesLabel { get; set; }
        public string DeleteLabel { get; set; }
        public string CategoryPictureLabel { get; set; }
        public IEnumerable<ParentCategoryViewModel> ParentCategories { get; set; }
    }
}
