using System;

namespace Seller.Web.Areas.Products.DomainModels
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int? Order { get; set; }
        public Guid MediaId { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
