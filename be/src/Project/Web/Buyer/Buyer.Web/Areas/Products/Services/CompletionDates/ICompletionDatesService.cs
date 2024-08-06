using Buyer.Web.Areas.Products.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Services.CompletionDates
{
    public interface ICompletionDatesService
    {
        Task<IEnumerable<Product>> GetCompletionDatesAsync(string token, string language, string userEmail, IEnumerable<Product> products);
        Task<Product> GetCompletionDateAsync(string token, string language, string userEmail, Product product);
    }
}
