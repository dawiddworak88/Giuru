using Foundation.ApiExtensions.Models.Response;
using Foundation.GenericRepository.Paginations;
using Foundation.TenantDatabase.Areas.Clients.Entities;
using System.Collections.Generic;

namespace Api.v1.Areas.Clients.ResponseModels
{
    public class ClientsResponseModel : BaseResponseModel
    {
        public PagedResults<IEnumerable<ClientResponseModel>> PagedClients { get; set; }

        public ClientsResponseModel()
        { 
        }

        public ClientsResponseModel(PagedResults<IEnumerable<Client>> pagedClients)
        {
            var clientsList = new List<ClientResponseModel>();

            foreach (var client in pagedClients.Data)
            {
                clientsList.Add(new ClientResponseModel { Client = client });
            }

            this.PagedClients = new PagedResults<IEnumerable<ClientResponseModel>>
            {
                Data = clientsList,
                PageCount = pagedClients.PageCount,
                Total = pagedClients.Total
            };
        }
    }
}
