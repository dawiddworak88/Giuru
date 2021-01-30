using System;

namespace Ordering.Api.ServicesModels
{
    public class OrderServiceModel
    {
        public Guid? Id { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
