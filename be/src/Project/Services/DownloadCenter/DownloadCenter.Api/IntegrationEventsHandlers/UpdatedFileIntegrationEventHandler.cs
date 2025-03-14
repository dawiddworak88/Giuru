﻿using DownloadCenter.Api.IntegrationEvents;
using DownloadCenter.Api.Services.DownloadCenter;
using Foundation.EventBus.Abstractions;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DownloadCenter.Api.IntegrationEventsHandlers
{
    public class UpdatedFileIntegrationEventHandler : IIntegrationEventHandler<UpdatedFileIntegrationEvent>
    {
        private readonly IDownloadCenterService downloadCenterService;

        public UpdatedFileIntegrationEventHandler(
            IDownloadCenterService downloadCenterService)
        {
            this.downloadCenterService = downloadCenterService;
        }

        /// <summary>
        /// Integration event handler which updates the file name
        /// </summary>
        /// <param name="event">
        /// Integration event message which is sent by the
        /// media.api once seller updated file name.
        /// </param>
        /// <returns></returns>
        public async Task Handle(UpdatedFileIntegrationEvent @event)
        {
            using var source = new ActivitySource(this.GetType().Name);
            using var activity = source.StartActivity($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {@event.GetType().Name}");

            await this.downloadCenterService.UpdateFileNameAsync(@event.FileId, @event.Name);
        }
    }
}
