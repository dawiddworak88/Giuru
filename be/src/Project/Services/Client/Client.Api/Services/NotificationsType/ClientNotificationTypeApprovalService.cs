using Client.Api.Infrastructure;
using Client.Api.Infrastructure.Notifications.Entities;
using Client.Api.ServicesModels.Notification;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Api.Services.NotificationsType
{
    public class ClientNotificationTypeApprovalService : IClientNotificationTypeApprovalService
    {
        private readonly ClientContext _context;

        public ClientNotificationTypeApprovalService(ClientContext context) 
        { 
            _context = context;
        }

        public IEnumerable<ClientNotificationTypeApprovalServiceModel> Get(GetClientNotificationTypeApprovalsServiceModel model)
        {
            var clientNotificationTypeApprovals = _context.ClientNotificationTypeApprovals.Where(x => x.ClientId == model.ClientId);

            if (clientNotificationTypeApprovals is not null) 
            {
                return clientNotificationTypeApprovals.Select(x => new ClientNotificationTypeApprovalServiceModel
                {
                    Id = x.Id,
                    ClientId = x.ClientId,
                    IsApproved = x.IsApproved,
                    ApprovalDate = x.ApprovalDate,
                    NotificationTypeId = x.ClientNotificationTypeId

                });
            }

            return default;
        }

        public async Task SaveAsync(SaveNotificationTypeApprovalServiceModel model)
        {
            var notificationTypeApprovals = _context.ClientNotificationTypeApprovals.Where(x => x.ClientId == model.ClientId);

            foreach (var clientApproval in notificationTypeApprovals.OrEmptyIfNull())
            {
                _context.ClientNotificationTypeApprovals.Remove(clientApproval);
            }

            foreach (var notificationTypeId in model.NotificationTypeIds.OrEmptyIfNull())
            {
                var clientNotificationTypeApproval = new ClientNotificationTypeApproval
                {
                    ClientId = model.ClientId,
                    IsApproved = true,
                    ApprovalDate = DateTime.UtcNow,
                    ClientNotificationTypeId = notificationTypeId,
                };

                await _context.ClientNotificationTypeApprovals.AddAsync(clientNotificationTypeApproval.FillCommonProperties());
            }

            await _context.SaveChangesAsync();
        }
    }
}
