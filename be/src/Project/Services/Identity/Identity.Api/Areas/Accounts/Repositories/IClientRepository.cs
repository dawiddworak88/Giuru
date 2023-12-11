using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.Repositories
{
    public interface IClientRepository
    {
        public Task<Guid> AddMarketingApprovals(string token, string language, Guid? id, IEnumerable<string> marketingApprovals);
        public Task<Guid?> GetClientByOrganistationId(string language, string token, Guid? id);
    }
}
