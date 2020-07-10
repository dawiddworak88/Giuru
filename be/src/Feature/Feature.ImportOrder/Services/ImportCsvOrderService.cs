using Feature.ImportOrder.DomainModels;
using System.IO;

namespace Feature.ImportOrder.Services
{
    public class ImportCsvOrderService : IImportOrderService
    {
        public Order ImportOrder(Stream stream)
        {
            return new Order();
        }
    }
}
