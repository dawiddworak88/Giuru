using Foundation.Extensions.Models;
using System;

namespace Analytics.Api.Shared.ServicesModels
{
    public class ChartBaseServiceModel : BaseServiceModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set;}
    }
}
