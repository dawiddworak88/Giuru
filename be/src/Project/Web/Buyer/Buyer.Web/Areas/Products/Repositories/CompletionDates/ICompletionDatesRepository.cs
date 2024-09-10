using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Shared.DomainModels.Clients;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Repositories.CompletionDates
{
    public interface ICompletionDatesRepository
    {
        Task<IEnumerable<Product>> PostAsync(string token, string language, IEnumerable<Product> products, IEnumerable<ClientFieldValue> clientFields, DateTime currentDate);
        Task<Product> PostAsync(string token, string language, Product product, IEnumerable<ClientFieldValue> clientFields, DateTime currentDate);
    }
}
