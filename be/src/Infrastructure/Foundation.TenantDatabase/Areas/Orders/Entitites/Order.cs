using Foundation.GenericRepository.Entities;
using System;

namespace Foundation.TenantDatabase.Areas.Orders.Entitites
{
    public class Order : Entity
    {
        public Guid ClientId { get; set; }
    }
}
