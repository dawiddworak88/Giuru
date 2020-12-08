using System;

namespace Seller.Web.Areas.Products.ViewModels
{
    public class ParentCategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
    }
}
