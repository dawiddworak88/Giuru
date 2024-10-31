using System;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ApprovalViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime ApprovalDate { get; set; }
        public bool IsApproved { get; set; }
    }
}
