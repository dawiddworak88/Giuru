using Seller.Web.Shared.DomainModels.LeadTime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Shared.Repositories.LeadTime
{
    public interface ILeadTimeRepository
    {
        Task<IEnumerable<LeadTimeItem>> GetLeadTimesAsync(string accessToken, Guid customerId, string[] skus);
    }
}
