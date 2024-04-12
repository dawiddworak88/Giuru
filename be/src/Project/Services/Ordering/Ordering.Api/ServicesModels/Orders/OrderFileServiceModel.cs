using System;

namespace Ordering.Api.ServicesModels.Orders
{
    public class OrderFileServiceModel
    {
        public Guid? Id { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
