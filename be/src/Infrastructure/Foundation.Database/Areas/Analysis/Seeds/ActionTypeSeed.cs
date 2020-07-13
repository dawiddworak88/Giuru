using Foundation.Database.Areas.Analysis.Definitions;
using Foundation.Database.Areas.Analysis.Entities;
using Foundation.Database.Shared.Contexts;
using Foundation.Database.Shared.Helpers;
using System.Linq;

namespace Foundation.Database.Shared.Seeds
{
    public static class ActionTypeSeed
    {
        public static void EnsureActionTypesSeeded(DatabaseContext context)
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
