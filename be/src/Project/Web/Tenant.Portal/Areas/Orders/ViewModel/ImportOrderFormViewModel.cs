using System.Collections.Generic;
using Tenant.Portal.Areas.Clients.DomainModels;

namespace Tenant.Portal.Areas.Orders.ViewModel
{
    public class ImportOrderFormViewModel
    {
        public IEnumerable<Client> Clients { get; set; }
        public string SaveText { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string SelectClientLabel { get; set; }
        public string DropFilesLabel { get; set; }
        public string DropOrSelectFilesLabel { get; set; }
    }
}
