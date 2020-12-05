using System.Collections.Generic;

namespace Seller.Web.Areas.Products.ViewModels
{
    public class CategoryDetailFormViewModel
    {
        public string Title { get; set; }
        public string NameRequiredErrorMessage { get; set; }
        public string SelectCategoryLabel { get; set; }
        public string ParentCategoryLabel { get; set; }
        public string NameLabel { get; set; }
        public string SaveText { get; set; }
        public string GeneralErrorMessage { get; set; }
        public IEnumerable<ParentCategoryViewModel> ParentCategories { get; set; }
    }
}
