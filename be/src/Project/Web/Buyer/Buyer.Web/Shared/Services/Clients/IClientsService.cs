using System;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Services.Clients
{
    public interface IClientsService
    {
        Task<bool> IsEltapTransportEnableAsync(string token, string language, Guid? sellerId);
    }
}
