using System;

namespace Foundation.PageContent.ComponentModels
{
    public class ComponentModelBase
    {
        public Guid? Id { get; set; }
        public Guid? BasketId { get; set; }
        public Guid? SellerId { get; set; }
        public string Language { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public bool IsAuthenticated { get; set; }
        public string ContentPageKey { get; set; }
    }
}
