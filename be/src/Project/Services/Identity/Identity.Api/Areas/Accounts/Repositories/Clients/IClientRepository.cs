using Identity.Api.Areas.Accounts.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.Repositories.Clients
{
    public interface IClientRepository
    {
        public Task<Client> GetByOrganisationAsync(string token, string language, Guid? id);
    }
}
