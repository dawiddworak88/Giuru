using System;

namespace Basket.Api.RepositoriesModels
{
    public class BasketItemRepositoryModel
    {
        public Guid? ProductId { get; set; }
        public double Quantity { get; set; }
        public DateTime? DeliveryFrom { get; set; }
        public DateTime? DeliveryTo { get; set; }
        public string MoreInfo { get; set; }
    }
}
