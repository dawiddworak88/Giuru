namespace Feature.ImportOrder.Services
{
    public interface IImportOrderServiceFactory
    {
        IImportOrderService GetImportOrderService(string filename);
    }
}
