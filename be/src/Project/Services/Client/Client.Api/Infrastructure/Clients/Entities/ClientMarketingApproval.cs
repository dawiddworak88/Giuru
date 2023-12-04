using Foundation.GenericRepository.Entities;
using System;

namespace Client.Api.Infrastructure.Clients.Entities
{
    public class ClientMarketingApproval : Entity
    {
        public string Name { get; set; }

        public bool IsApproved { get; set; }

        public Guid? ClientId { get; set; }
    }
}
