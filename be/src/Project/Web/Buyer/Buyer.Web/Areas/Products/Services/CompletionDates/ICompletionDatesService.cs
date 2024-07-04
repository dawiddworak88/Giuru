using Buyer.Web.Areas.Products.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Services.CompletionDates
{
    public interface ICompletionDatesService
    {
        Task<List<Product>> GetCompletionDatesAsync(string token, string language, Guid? clientId, List<Product> products);
    }
}
