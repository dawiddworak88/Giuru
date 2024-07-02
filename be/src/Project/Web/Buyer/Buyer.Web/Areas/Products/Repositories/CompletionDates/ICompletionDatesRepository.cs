using System;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Repositories.CompletionDates
{
    public interface ICompletionDatesRepository
    {
        Task<int> GetAsync(string token, string language, Guid transportId, Guid conditionId, Guid? zoneId, Guid? campaignId, DateTime currentDate);
    }
}
