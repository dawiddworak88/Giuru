using Buyer.Web.Shared.DomainModels.LeadTime;
using System;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.LeadTime
{
    public interface ILeadTimeRepository
    {
        Task<PagedLeadTimeResults> GetLeadTimesAsync(string accessToken, string[] skus);
    }
}
