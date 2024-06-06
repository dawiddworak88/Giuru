using System.Collections.Generic;

namespace Seller.Web.Areas.Settings.ViewModels
{
    public class SettingsFormViewModel 
    { 
        public string Title { get; set; }
        public string ReindexProductsText { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string ProductsIndexTriggerUrl { get; set; }
        public string SaveUrl { get; set; }
        public string SaveText { get; set; }
        public string ExternalCompletionDatesText { get; set; }
        public Dictionary<string, string> Settings { get; set; }
    }
}
