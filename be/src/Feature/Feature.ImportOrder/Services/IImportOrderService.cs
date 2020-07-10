using Feature.ImportOrder.DomainModels;
using System.IO;

namespace Feature.ImportOrder.Services
{
    public interface IImportOrderService
    {
        Order ImportOrder(Stream stream);
    }
}
