using Buyer.Web.Areas.Orders.DomainModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Orders.Services.OrderFiles
{
    public interface IOrderFileService
    {
        IEnumerable<OrderFileLine> ImportOrderLines(IFormFile file);
    }
}
