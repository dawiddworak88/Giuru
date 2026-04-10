using Buyer.Web.Shared.DomainModels.LeadTime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.LeadTime
{
    public interface ILeadTimeRepository
    {
        Task<IEnumerable<LeadTimeItem>> GetLeadTimesAsync(string accessToken, string[] skus);
    }
}
