using System;

namespace Seller.Web.Areas.Products.ViewModels
{
    public class ProductCardFormViewModel
    {
        public Guid? Id { get; set; }
        public string IdLabel { get; set; }
        public string Title { get; set; }
        public string NewText { get; set; }
        public string SaveText { get; set; }
        public string SaveUrl { get; set; }
        public string Schema { get; set; }
        public string UiSchema { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string NavigateToProductCardsLabel { get; set; }
        public string FieldRequiredErrorMessage { get; set; }
        public string ProductCardsUrl { get; set; }
        public string DefinitionUrl { get; set; }
        public string DefaultInputName { get; set; }
        public string YesLabel { get; set; }
        public string NoLabel { get; set; }
        public string AreYouSureLabel { get; set; }
        public string DeleteConfirmationLabel { get; set; }
        public ProductCardModalViewModel ProductCardModal { get; set; }
    }
}
