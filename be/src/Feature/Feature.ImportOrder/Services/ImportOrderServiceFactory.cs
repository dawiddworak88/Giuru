using System.IO;

namespace Feature.ImportOrder.Services
{
    public class ImportOrderServiceFactory : IImportOrderServiceFactory
    {
        public IImportOrderService GetImportOrderService(string filename)
        {
            IImportOrderService importOrderService = null;

            var extension = Path.GetExtension(filename);

            if (!string.IsNullOrWhiteSpace(extension))
            {
                switch (extension)
                {
                    case ".xlsx":
                    case ".xls":
                        importOrderService = new ImportXlOrderService();
                        break;
                    case ".csv":
                        importOrderService = new ImportCsvOrderService();
                        break;
                }
            }

            return importOrderService;
        }
    }
}
