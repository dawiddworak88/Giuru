using Foundation.PageContent.Components.ListItems.ViewModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.News.ViewModel
{
    public class CategoryFormViewModel
    {
        public string Title { get; set; }
        public Guid? Id { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string Name { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string NameLabel { get; set; }
        public string ParentCategoryLabel { get; set; }
        public string SelectCategoryLabel { get; set; }
        public string NameRequiredErrorMessage { get; set; }
        public string NavigateToCategoriesLabel { get; set; }
        public string SaveUrl { get; set; }
        public string CategoriesUrl { get; set; }
        public string SaveText { get; set; }
        public IEnumerable<ListItemViewModel> ParentCategories { get; set; }
    }
}
