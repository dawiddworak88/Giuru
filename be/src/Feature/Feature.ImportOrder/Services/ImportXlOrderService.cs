using ClosedXML.Excel;
using Feature.ImportOrder.DomainModels;
using System.Collections.Generic;
using System.IO;

namespace Feature.ImportOrder.Services
{
    public class ImportXlOrderService : IImportOrderService
    {
        public Order ImportOrder(Stream stream)
        {
            var workbook = new XLWorkbook(stream);

            var orderItems = new List<OrderItem>();

            return new Order
            { 
                OrderItems = orderItems
            };
        }
    }
}
