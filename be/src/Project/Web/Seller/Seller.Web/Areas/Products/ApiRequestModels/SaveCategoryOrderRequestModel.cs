using System;

namespace Seller.Web.Areas.Products.ApiRequestModels
{
    public class SaveCategoryOrderRequestModel
    {
        public Guid? Id { get; set; }
        public int Order { get; set; }
    }
}
