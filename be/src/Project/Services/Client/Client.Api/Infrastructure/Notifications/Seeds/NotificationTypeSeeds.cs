using Client.Api.Infrastructure.Notifications.Entities;
using System.Linq;
using System;
using System.Linq.Dynamic.Core;
using Foundation.GenericRepository.Extensions;
using Client.Api.Definitions;

namespace Client.Api.Infrastructure.Notifications.Seeds
{
    public static class NotificationTypeSeeds
    {
        public static void SeedNotificationTypes(ClientContext context)
        {
            SeedNotificationType(context, NotificationApprovalConstants.MarketingInformationBySMSId, "Approval sending marketing informations by SMS", "Zgoda na przesyłanie informacji marketingowych za pomocą SMS", "Einwilligung zum Erhalt von Marketinginformationen per SMS");
            SeedNotificationType(context, NotificationApprovalConstants.MarketingInformationByEmailId, "Approval sending marketing informations by Email", "Zgoda na przesyłanie informacji marketingowych za pomocą Email", "Einwilligung zum Erhalt von Marketinginformationen per Email");
            SeedNotificationType(context, NotificationApprovalConstants.SendingOrderConfirmationId, "Approval for sending order confirmation emails", "Zgoda na wysyłanie Emaili potwierdzających złożenie zamówienia", "Einwilligung zur Zusendung von Bestellbestätigungs-E-Mails");
        }
        private static void SeedNotificationType(ClientContext context, Guid id, string englishName, string polishName, string germanName)
        {
            if (!context.ClientNotificationTypes.Any(x => x.Id == id))
            {
                var notificationType = new ClientNotificationType
                {
                    Id = id,
                };

                var enNotificationTypeTranslation = new ClientNotificationTypeTranslations
                {
                    Name = englishName,
                    ClientNotificationTypeId = id,
                    Language = "en",
                };

                var plNotificationTypeTranslation = new ClientNotificationTypeTranslations
                {
                    Name = polishName,
                    ClientNotificationTypeId = id,
                    Language = "pl",
                };

                var deNotificationTypeTranslation = new ClientNotificationTypeTranslations
                {
                    Name = germanName,
                    ClientNotificationTypeId = id,
                    Language = "de",
                };

                context.ClientNotificationTypes.Add(notificationType.FillCommonProperties());
                context.ClientNotificationTypeTranslations.Add(enNotificationTypeTranslation.FillCommonProperties());
                context.ClientNotificationTypeTranslations.Add(plNotificationTypeTranslation.FillCommonProperties());
                context.ClientNotificationTypeTranslations.Add(deNotificationTypeTranslation.FillCommonProperties());

                context.SaveChanges();
            }
        }
    }
}
