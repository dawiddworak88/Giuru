using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientFieldViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsRequired { get; set; }
        public IEnumerable<ClientFieldOptionViewModel> Options { get; set; }
    }
}
