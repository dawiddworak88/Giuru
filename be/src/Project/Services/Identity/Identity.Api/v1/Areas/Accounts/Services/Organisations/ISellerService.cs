using Identity.Api.v1.Areas.Accounts.Models;
using Identity.Api.v1.Areas.Accounts.ResultModels;
using System.Threading.Tasks;

namespace Identity.Api.v1.Areas.Accounts.Services.Organisations
{
    public interface ISellerService
    {
        Task<SellerResultModel> GetAsync(GetSellerModel serviceModel);
    }
}
