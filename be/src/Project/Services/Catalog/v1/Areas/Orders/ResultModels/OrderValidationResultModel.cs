using Foundation.Extensions.Models;
using System.Collections.Generic;

namespace Catalog.Api.v1.Areas.Schemas.ResultModels
{
    public class OrderValidationResultModel : BaseServiceResultModel
    {
        public OrderValidationResultModel()
        {
            this.ValidationMessages = new List<string>();
        }

        public List<string> ValidationMessages { get; set; }
    }
}
