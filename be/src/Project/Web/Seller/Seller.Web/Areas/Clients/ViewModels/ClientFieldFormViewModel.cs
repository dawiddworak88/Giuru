using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientFieldFormViewModel
    {
        public Guid? Id { get; set; }
        public string IdLabel { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string NameLabel { get; set; }
        public string TypeLabel { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string FieldRequiredErrorMessage { get; set; }
        public string SaveText { get; set; }
        public string SaveUrl { get; set; }
        public string EditUrl { get; set; }
        public string ClientFieldsUrl { get; set; }
        public string NavigateToFieldsText { get; set; }
        public IEnumerable<ClientFieldTypeViewModel> Types { get; set; }
    }
}
