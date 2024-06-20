using Buyer.Web.Areas.Products.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Services.CompletionDates
{
    public interface ICompletionDatesService
    {
        Task GetCompletionDatesAsync(string token, string language, List<Product> products, Guid? clientId);
        Task GetCompletionDatesAsync(string token, string language, Product product, Guid? clientId);
    }
}
