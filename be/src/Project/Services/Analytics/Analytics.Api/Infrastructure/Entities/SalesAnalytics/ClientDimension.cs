using Foundation.GenericRepository.Entities;
using System;

namespace Analytics.Api.Infrastructure.Entities.SalesAnalytics
{
    public class ClientDimension : Entity
    {
        public Guid ClientId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
