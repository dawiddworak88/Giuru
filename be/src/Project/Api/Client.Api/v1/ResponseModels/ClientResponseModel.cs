using System;

namespace Client.Api.v1.ResponseModels
{
    public class ClientResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ClientResponseModel(Foundation.TenantDatabase.Areas.Clients.Entities.Client client)
        {
            this.Id = client.Id;
            this.Name = client.Name;
        }
    }
}
