using Foundation.TenantDatabase.Areas.Analysis.Definitions;
using Foundation.TenantDatabase.Areas.Analysis.Entities;
using Foundation.TenantDatabase.Shared.Contexts;
using Foundation.TenantDatabase.Shared.Helpers;
using System.Linq;

namespace Foundation.TenantDatabase.Shared.Seeds
{
    public static class ActionTypeSeed
    {
        public static void EnsureActionTypesSeeded(this TenantDatabaseContext context)
        {
            // Adds action types
            if (!context.ActionTypes.Any(x => x.Name == ActionTypeConstants.NewOrder))
            {
                var newOrder = EntitySeedHelper.SeedEntity(new ActionType { Id = ActionTypeConstants.NewOrderId, Name = ActionTypeConstants.NewOrder });

                context.ActionTypes.Add(newOrder);

                context.SaveChanges();
            }

            if (!context.ActionTypes.Any(x => x.Name == ActionTypeConstants.NewOrderItem))
            {
                var newOrderItemId = EntitySeedHelper.SeedEntity(new ActionType { Id = ActionTypeConstants.NewOrderItemId, Name = ActionTypeConstants.NewOrderItem });

                context.ActionTypes.Add(newOrderItemId);

                context.SaveChanges();
            }

            if (!context.ActionTypes.Any(x => x.Name == ActionTypeConstants.ChangeOrderItemStatus))
            {
                var changeOrderStatus = EntitySeedHelper.SeedEntity(new ActionType { Id = ActionTypeConstants.ChangeOrderItemStatusId, Name = ActionTypeConstants.ChangeOrderItemStatus });

                context.ActionTypes.Add(changeOrderStatus);

                context.SaveChanges();
            }

            if (!context.ActionTypes.Any(x => x.Name == ActionTypeConstants.NewComplaint))
            {
                var newComplaint = EntitySeedHelper.SeedEntity(new ActionType { Id = ActionTypeConstants.NewComplaintId, Name = ActionTypeConstants.NewComplaint });

                context.ActionTypes.Add(newComplaint);

                context.SaveChanges();
            }
        }
    }
}
