using Buyer.Web.Areas.Dashboard.DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Dashboard.Repositories
{
    public interface ISalesAnalyticsRepository
    {
        Task<IEnumerable<AnnualSalesItem>> GetAnnualSales(string token, string language);
    }
}
