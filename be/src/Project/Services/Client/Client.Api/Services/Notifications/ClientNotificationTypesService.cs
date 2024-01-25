using Client.Api.Infrastructure;
using Client.Api.ServicesModels.Notification;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Linq;
using Client.Api.Infrastructure.Notifications.Entities;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Definitions;
using Microsoft.EntityFrameworkCore;
using Foundation.Extensions.Exceptions;
using System.Net;
using Foundation.GenericRepository.Extensions;
using System;

namespace Client.Api.Services.Notifications
{
    public class ClientNotificationTypesService : IClientNotificationTypesService
    {
        private readonly ClientContext _context;
        private readonly IStringLocalizer _clientLocalizer;

        public ClientNotificationTypesService(
            ClientContext context,
            IStringLocalizer<ClientResources> clientLocalizer)
        {
            _context = context;
            _clientLocalizer = clientLocalizer;
        }
        
        public PagedResults<IEnumerable<ClientNotificationTypeServiceModel>> Get(GetClientNotificationTypesServiceModel model)
        {
            var notificationTypes = _context.ClientNotificationTypes.Where(x => x.IsActive)
                .Include(x => x.Translations)
                .AsSingleQuery();

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                notificationTypes = notificationTypes.Where(x => x.Translations.Any(x => x.Name.StartsWith(model.SearchTerm)));
            }

            notificationTypes = notificationTypes.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<ClientNotificationType>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                notificationTypes = notificationTypes.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = notificationTypes.PagedIndex(new Pagination(notificationTypes.Count(), Constants.MaxItemsPerPage), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = notificationTypes.PagedIndex(new Pagination(notificationTypes.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            return new PagedResults<IEnumerable<ClientNotificationTypeServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = notificationTypes.OrEmptyIfNull().Select(x => new ClientNotificationTypeServiceModel
                {
                    Id = x.Id,
                    Name = x.Translations.FirstOrDefault(t => t.ClientNotoficationTypeId == x.Id && t.Language == model.Language && t.IsActive)?.Name ?? x.Translations.FirstOrDefault(t => t.ClientNotoficationTypeId == x.Id && t.IsActive)?.Name,
                    CreatedDate = x.CreatedDate,
                    LastModifiedDate = x.LastModifiedDate
                })
            };
        }

        public async Task<ClientNotificationTypeServiceModel> GetAsync(GetClientNotificationTypeServiceModel model)
        {
            var notificationType = await _context.ClientNotificationTypes
                .Include(x => x.Translations)
                .AsSingleQuery()
                .FirstOrDefaultAsync(x => x.IsActive && x.Id == model.Id);

            if (notificationType is null)
            {
                throw new CustomException(_clientLocalizer.GetString("NotificationTypeNotFound"), (int)HttpStatusCode.NoContent);
            }

            return new ClientNotificationTypeServiceModel
            {
                Id = notificationType.Id,
                Name = notificationType.Translations.FirstOrDefault(t => t.ClientNotoficationTypeId == notificationType.Id && t.Language == model.Language && t.IsActive)?.Name ?? notificationType.Translations.FirstOrDefault(t => t.ClientNotoficationTypeId == notificationType.Id && t.IsActive)?.Name,
                CreatedDate = notificationType.CreatedDate,
                LastModifiedDate = notificationType.LastModifiedDate
            };
        }

        public async Task<ClientNotificationTypeServiceModel> CreateAsync(CreateClientNotificationTypeServiceModel model)
        {
            var notificationType = new ClientNotificationType();

            _context.ClientNotificationTypes.Add(notificationType.FillCommonProperties());

            var notificationTypeTranslation = new ClientNotificationTypeTranslations
            {
                Name = model.Name,
                Language = model.Language,
                ClientNotoficationTypeId = notificationType.Id
            };

            _context.ClientNotificationTypeTranslations.Add(notificationTypeTranslation.FillCommonProperties());

            await _context.SaveChangesAsync();

            return await GetAsync(new GetClientNotificationTypeServiceModel { Id = notificationType.Id, Language = model.Language, Username = model.Username, OrganisationId = model.OrganisationId });
        }

        public async Task DeleteAsync(DeleteClientNotificationTypeServiceModel model)
        {
            var notificationType = await _context.ClientNotificationTypes.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (notificationType is null)
            {
                throw new CustomException(_clientLocalizer.GetString("NotificationTypeNotFound"), (int)HttpStatusCode.NoContent);
            }

            notificationType.IsActive = false;

            await _context.SaveChangesAsync();
        }

        public async Task<ClientNotificationTypeServiceModel> UpdateAsync(UpdateClientNotificationTypeServiceModel model)
        {
            var notificationType = await _context.ClientNotificationTypes.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (notificationType is null)
            {
                throw new CustomException(_clientLocalizer.GetString("NotificationTypeNotFound"), (int)HttpStatusCode.NoContent);
            }

            var notificationTypeTranslation = _context.ClientNotificationTypeTranslations.FirstOrDefault(x => x.ClientNotoficationTypeId == model.Id && x.Language == model.Language && x.IsActive);

            if (notificationTypeTranslation is not null)
            {
                notificationTypeTranslation.Name = model.Name;
                notificationTypeTranslation.LastModifiedDate = DateTime.UtcNow;
            }
            else
            {
                var newNotificationTypeTranslation = new ClientNotificationTypeTranslations
                {
                    Name = model.Name,
                    Language = model.Language,
                };

                _context.ClientNotificationTypeTranslations.Add(newNotificationTypeTranslation.FillCommonProperties());
            }

            notificationType.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetAsync(new GetClientNotificationTypeServiceModel { Id = notificationType.Id, Language = model.Language, OrganisationId = model.OrganisationId, Username = model.Username });
        }
    }
}
