using Foundation.PageContent.Components.ListItems.ViewModels;
using Seller.Web.Shared.ViewModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Products.ViewModels
{
    public class ProductFormViewModel
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Schema { get; set; }
        public string UiSchema { get; set; }
        public string FormData { get; set; }
        public bool IsNew { get; set; }
        public bool IsPublished { get; set; }
        public string Ean { get; set; }
        public Guid? Id { get; set; }
        public Guid? CategoryId { get; set; }
        public IEnumerable<Guid> GroupIds { get; set; }
        public ListItemViewModel PrimaryProduct { get; set; }
        public IEnumerable<FileViewModel> Images { get; set; }
        public IEnumerable<FileViewModel> Files { get; set; }
        public ProductBaseFormViewModel ProductBase { get; set; }
    }
}