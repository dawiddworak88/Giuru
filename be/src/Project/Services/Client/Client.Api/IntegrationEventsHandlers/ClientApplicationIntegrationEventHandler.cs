using Client.Api.IntegrationEvents;
using Client.Api.Services.Applications;
using Client.Api.ServicesModels.Applications;
using Client.Api.Validators.Applications;
using Foundation.EventBus.Abstractions;
using System.Threading.Tasks;

namespace Client.Api.IntegrationEventsHandlers
{
    public class ClientApplicationIntegrationEventHandler : IIntegrationEventHandler<ClientApplicationIntegrationEvent>
    {
        private readonly IClientsApplicationsService clientsApplicationsService;

        public ClientApplicationIntegrationEventHandler(
            IClientsApplicationsService clientsApplicationsService)
        {
            this.clientsApplicationsService = clientsApplicationsService;
        }

        /// <summary>
        /// Integration event handler which saving
        /// </summary>
        /// <param name="event">
        /// Integration event message which is sent by the
        /// basket.api once it has successfully process the 
        /// order items.
        /// </param>
        /// <returns></returns>
        public async Task Handle(ClientApplicationIntegrationEvent @event)
        {
            var clientApplicationServiceModel = new CreateClientApplicationServiceModel
            {
                FirstName = @event.FirstName,
                LastName = @event.LastName,
                ContactJobTitle = @event.ContactJobTitle,
                PhoneNumber = @event.PhoneNumber,
                Email = @event.Email,
                CompanyAddress = @event.CompanyAddress,
                CompanyName = @event.CompanyName,
                CompanyCity = @event.CompanyCity,
                CompanyCountry = @event.CompanyCountry,
                CompanyPostalCode = @event.CompanyPostalCode,
                CompanyRegion = @event.CompanyRegion
            };

            var validator = new CreateClientApplicationModelValidator();

            var validationResult = await validator.ValidateAsync(clientApplicationServiceModel);

            if (validationResult.IsValid)
            {
                await this.clientsApplicationsService.CreateAsync(clientApplicationServiceModel);
            }
        }
    }
}
