using System;

namespace Seller.Web.Areas.Products.ViewModels
{
    public class ProductCardFormViewModel
    {
        public Guid? Id { get; set; }
        public string IdLabel { get; set; }
        public string Title { get; set; }
        public string SaveUrl { get; set; }
        public string SaveText { get; set; }
        public string Schema { get; set; }
        public string UiSchema { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string NavigateToProductCardsLabel { get; set; }
    }
}
