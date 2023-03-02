using Buyer.Web.Areas.Dashboard.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Dashboard.Repositories
{
    public interface ISalesAnalyticsRepository
    {
        Task<IEnumerable<AnnualSalesItem>> GetAnnualSales(string token, string language, DateTime? fromDate, DateTime? toDate);
    }
}
