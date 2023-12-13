using System;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientFieldOptionFormViewModel
    {
        public string Title { get; set; }
        public Guid? FieldDefinitionId { get; set; }
        public Guid? Id { get; set; }
        public string IdLabel { get; set; }
        public string Name { get; set; }
        public string NameLabel { get; set; }
        public string Value { get; set; }
        public string ValueLabel { get; set; }
        public string FieldRequiredErrorMessage { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string SaveUrl { get; set; }
        public string SaveText { get; set; }
        public string ClientFieldUrl { get; set; }
        public string NavigateToFieldText { get; set; }
    }
}
