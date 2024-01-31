using Seller.Web.Areas.Clients.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Repositories.FieldValues
{
    public interface IClientFieldValuesRepository
    {
        Task<IEnumerable<ClientFieldValue>> GetAsync(string token, string language, Guid? clientId);
        Task SaveAsync(string token, string language, Guid? clientId, IEnumerable<ApiClientFieldValue> fieldsValues);
    }
}
