using Buyer.Web.Areas.Orders.Definitions;
using Buyer.Web.Areas.Orders.DomainModels;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Buyer.Web.Areas.Orders.Services.OrderFiles
{
    public class OrderFileService : IOrderFileService
    {
        public IEnumerable<OrderFileLine> ImportOrderLines(IFormFile file)
        {
            var orderLines = new List<OrderFileLine>();

            var wb = new XLWorkbook(file.OpenReadStream());

            var worksheet = wb.Worksheet(OrderFileConstants.OrderItemsWorksheetIndex);

            var rows = worksheet.RangeUsed().RowsUsed().Skip(OrderFileConstants.HeaderRowIndex);

            foreach (var row in rows)
            {
                var orderLine = new OrderFileLine
                {
                    Sku = row.Cell(OrderFileConstants.SkuColumnIndex).GetValue<string>(),
                    Quantity = row.Cell(OrderFileConstants.QuantityColumnIndex).GetValue<double>(),
                    ExternalReference = row.Cell(OrderFileConstants.ExternalReferenceColumnIndex).GetValue<string>(),
                    MoreInfo = row.Cell(OrderFileConstants.MoreInfoColumnIndex).GetValue<string>()
                };

                orderLines.Add(orderLine);
            }

            return orderLines;
        }
    }
}
