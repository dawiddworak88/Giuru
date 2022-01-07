using Microsoft.AspNetCore.Http;
using Seller.Web.Areas.Orders.DomainModels;
using System.Collections.Generic;

namespace Seller.Web.Areas.Orders.Services.OrderFiles
{
    public interface IOrderFileService
    {
        IEnumerable<OrderFileLine> ImportOrderLines(IFormFile file);
    }
}
