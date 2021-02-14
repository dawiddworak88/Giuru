using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.EventBus.Events;
using Foundation.EventLog.ApiRequestModels;
using Foundation.EventLog.Definitions;
using Foundation.GenericRepository.EntitiesLog;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.EventLog.Repositories
{
    public class EventLogRepository : IEventLogRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IConfiguration configuration;

        public EventLogRepository(
            IApiClientService apiClientService,
            IConfiguration configuration)
        {
            this.apiClientService = apiClientService;
            this.configuration = configuration;
        }

        public async Task SaveAsync(IntegrationEvent @event, string eventState)
        {
            var requestModel = new SaveEventApiRequestModel
            {
                EventId = @event.Id,
                EventState = eventState,
                EventName = @event.EventName,
                Content = JsonConvert.SerializeObject(@event),
                Username = @event.Username,
                OrganisationId = @event.OrganisationId,
                Source = @event.Source,
                IpAddress = @event.IpAddress
            };

            var apiRequest = new ApiRequest<SaveEventApiRequestModel>
            {
                Data = requestModel,
                EndpointAddress = $"{this.configuration["EventLoggingUrl"]}{ApiConstants.EventLoggingApiEndpoint}"
            };

            await this.apiClientService.PostAsync<ApiRequest<SaveEventApiRequestModel>, SaveEventApiRequestModel, BaseResponseModel>(apiRequest);
        }

        public async Task SaveAsync(
            Guid entityId, 
            string entityType, 
            IEnumerable<EntityLogProperty> modifiedProperties,
            Guid organisationId,
            string username)
        {
            var requestModel = new SaveEventApiRequestModel
            {
                EntityId = entityId,
                EntityType = entityType,
                Content = string.Join(",", modifiedProperties.Select(x => x.PropertyName)),
                OldValue = string.Join(",", modifiedProperties.Select(x => x.OldValue)),
                NewValue = string.Join(",", modifiedProperties.Select(x => x.NewValue)),
                OrganisationId = organisationId,
                Username = username
            };

            var apiRequest = new ApiRequest<SaveEventApiRequestModel>
            {
                Data = requestModel,
                EndpointAddress = $"{this.configuration["EventLoggingUrl"]}{ApiConstants.EventLoggingApiEndpoint}"
            };

            await this.apiClientService.PostAsync<ApiRequest<SaveEventApiRequestModel>, SaveEventApiRequestModel, BaseResponseModel>(apiRequest);
        }
    }
}
