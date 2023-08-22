using Foundation.PageContent.Components.ListItems.ViewModels;
using Seller.Web.Areas.Products.DomainModels;
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
        public CategorySchemas Schemas { get; set; }
        public string UiSchema { get; set; }
        public CategoryBaseFormViewModel CategoryBase { get; set; }
    }
}
