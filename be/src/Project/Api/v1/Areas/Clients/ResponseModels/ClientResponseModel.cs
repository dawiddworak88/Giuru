using Foundation.ApiExtensions.Models.Response;
using Foundation.TenantDatabase.Areas.Clients.Entities;
using System;

namespace Api.v1.Areas.Clients.ResponseModels
{
    public class ClientResponseModel : BaseResponseModel
    {
        public Guid Id { get; set; }
        public string Host { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public ClientResponseModel(Client client)
        {
            this.Id = client.Id;
            this.Host = client.Host;
            this.Name = client.Name;
            this.Language = client.Language;
            this.LastModifiedDate = client.LastModifiedDate;
            this.CreatedDate = client.CreatedDate;
        }

        public ClientResponseModel()
        { 
        }
    }
}
