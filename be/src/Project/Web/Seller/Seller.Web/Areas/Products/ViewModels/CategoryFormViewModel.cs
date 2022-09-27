using Foundation.PageContent.Components.ListItems.ViewModels;
using Seller.Web.Shared.ViewModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Products.ViewModels
{
    public class CategoryFormViewModel
    {
        public Guid? Id { get; set; }
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
        public string SaveMediaUrl { get; set; }
        public string SaveUrl { get; set; }
        public string CategoriesUrl { get; set; }
        public string NavigateToCategoriesLabel { get; set; }
        public string IdLabel { get; set; }
        public string Schema { get; set; }
        public string UiSchema { get; set; }
        public IEnumerable<ListItemViewModel> ParentCategories { get; set; }
    }
}
