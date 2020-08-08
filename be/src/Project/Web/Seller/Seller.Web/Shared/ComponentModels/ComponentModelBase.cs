using System;

namespace Seller.Web.Shared.ComponentModels
{
    public class ComponentModelBase
    {
        public Guid? Id { get; set; }
        public string Language { get; set; }
        public string Token { get; set; }
    }
}
