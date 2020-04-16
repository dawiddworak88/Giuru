using FluentValidation.Results;

namespace Feature.Client.Models
{
    public class CreateClientResultModel
    {
        public Foundation.TenantDatabase.Areas.Clients.Entities.Client Client { get; set; }
        public ValidationResult ValidationResult { get; set; }
    }
}
