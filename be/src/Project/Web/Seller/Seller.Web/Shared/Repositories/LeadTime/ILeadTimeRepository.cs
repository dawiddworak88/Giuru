using Seller.Web.Shared.DomainModels.LeadTime;
using System;
using System.Threading.Tasks;

namespace Seller.Web.Shared.Repositories.LeadTime
{
    public interface ILeadTimeRepository
    {
        Task<PagedLeadTimeResults> GetLeadTimesAsync(string accessToken, Guid customerId, string[] skus);
    }
}
