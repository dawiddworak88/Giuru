using Foundation.GenericRepository.Entities;
using System;

namespace Foundation.Database.Areas.Orders.Entitites
{
    public class Order : Entity
    {
        public Guid ClientId { get; set; }
    }
}
