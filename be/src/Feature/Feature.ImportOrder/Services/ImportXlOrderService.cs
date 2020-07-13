using ClosedXML.Excel;
using Feature.ImportOrder.DomainModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Feature.ImportOrder.Services
{
    public class ImportXlOrderService : IImportOrderService
    {
        public Order ImportOrder(Stream stream)
        {
            var orderItems = new List<OrderItem>();

            var workbook = new XLWorkbook(stream);

            var orderWorksheet = workbook.Worksheet("Order");

            if (orderWorksheet != null)
            { 
                var nonEmptyRows = orderWorksheet.RowsUsed();

                for (int i = 2; i < nonEmptyRows.Count(); i++)
                {
                }
            }

            return new Order
            { 
                OrderItems = orderItems
            };
        }
    }
}
