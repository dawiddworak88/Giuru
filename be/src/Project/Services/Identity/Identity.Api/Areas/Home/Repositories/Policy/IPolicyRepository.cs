using System.Threading.Tasks;
using Identity.Api.Areas.Home;

namespace Identity.Api.Areas.Home.Repositories.Policy
{
    public interface IPolicyRepository
    {
        Task<DomainModels.Policy> GetPolicyAsync(string language);
    }
}
