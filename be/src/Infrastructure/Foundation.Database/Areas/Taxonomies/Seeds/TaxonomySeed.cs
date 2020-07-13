using Foundation.Database.Areas.Orders.Definitions;
using Foundation.Database.Areas.Taxonomies.Entities;
using Foundation.Database.Areas.Translations.Definitions.Languages;
using Foundation.Database.Areas.Translations.Entities;
using Foundation.Database.Shared.Contexts;
using Foundation.Database.Shared.Helpers;
using System.Linq;

namespace Foundation.Database.Shared.Seeds
{
    public static class TaxonomySeed
    {
        public static void EnsureTaxonomiesSeeded(DatabaseContext context)
        {
            // Adds order statuses
            if (!context.Taxonomies.Any(x => x.Name == OrderStatusConstants.OrderStatusTaxonomyName))
            {
                // Order statuses
                var orderStatusesTaxonomy = EntitySeedHelper.SeedEntity(new Taxonomy { Id = OrderStatusConstants.OrderStatusesTaxonomyId, Name = OrderStatusConstants.OrderStatusTaxonomyName });

                context.Taxonomies.Add(orderStatusesTaxonomy);

                var enOrderStatus = EntitySeedHelper.SeedEntity(new Translation { Key = orderStatusesTaxonomy.Id.ToString(), Language = LanguageConstants.English, Value = "Order statuses" });
                var deOrderStatus = EntitySeedHelper.SeedEntity(new Translation { Key = orderStatusesTaxonomy.Id.ToString(), Language = LanguageConstants.German, Value = "Bestellstatus" });
                var plOrderStatus = EntitySeedHelper.SeedEntity(new Translation { Key = orderStatusesTaxonomy.Id.ToString(), Language = LanguageConstants.Polish, Value = "Status zamówienia" });

                context.Translations.Add(enOrderStatus);
                context.Translations.Add(deOrderStatus);
                context.Translations.Add(plOrderStatus);

                context.SaveChanges();

                // New
                var newOrderStatusesTaxonomy = EntitySeedHelper.SeedEntity(new Taxonomy { Id = OrderStatusConstants.NewOrderStatusTaxonomyId, Name = OrderStatusConstants.NewOrderStatusName, ParentId = orderStatusesTaxonomy.Id, Order = 100 });

                context.Taxonomies.Add(newOrderStatusesTaxonomy);

                var enNewOrderStatus = EntitySeedHelper.SeedEntity(new Translation { Key = newOrderStatusesTaxonomy.Id.ToString(), Language = LanguageConstants.English, Value = "New" });
                var deNewOrderStatus = EntitySeedHelper.SeedEntity(new Translation { Key = newOrderStatusesTaxonomy.Id.ToString(), Language = LanguageConstants.German, Value = "Neu" });
                var plNewOrderStatus = EntitySeedHelper.SeedEntity(new Translation { Key = newOrderStatusesTaxonomy.Id.ToString(), Language = LanguageConstants.Polish, Value = "Nowe" });

                context.Translations.Add(enNewOrderStatus);
                context.Translations.Add(deNewOrderStatus);
                context.Translations.Add(plNewOrderStatus);

                context.SaveChanges();

                // Processing
                var processingOrderStatusesTaxonomy = EntitySeedHelper.SeedEntity(new Taxonomy { Id = OrderStatusConstants.ProcessingOrderStatusTaxonomyId, Name = OrderStatusConstants.ProcessingOrderStatusName, ParentId = orderStatusesTaxonomy.Id, Order = 200 });

                context.Taxonomies.Add(processingOrderStatusesTaxonomy);

                var enProcessingOrderStatus = EntitySeedHelper.SeedEntity(new Translation { Key = processingOrderStatusesTaxonomy.Id.ToString(), Language = LanguageConstants.English, Value = "Processing" });
                var deProcessingOrderStatus = EntitySeedHelper.SeedEntity(new Translation { Key = processingOrderStatusesTaxonomy.Id.ToString(), Language = LanguageConstants.German, Value = "in Bearbeitung" });
                var plProcessingOrderStatus = EntitySeedHelper.SeedEntity(new Translation { Key = processingOrderStatusesTaxonomy.Id.ToString(), Language = LanguageConstants.Polish, Value = "W trakcie realizacji" });

                context.Translations.Add(enProcessingOrderStatus);
                context.Translations.Add(deProcessingOrderStatus);
                context.Translations.Add(plProcessingOrderStatus);

                context.SaveChanges();

                // Complete
                var completeOrderStatusesTaxonomy = EntitySeedHelper.SeedEntity(new Taxonomy { Id = OrderStatusConstants.CompleteOrderStatusTaxonomyId, Name = OrderStatusConstants.CompleteOrderStatusName, ParentId = orderStatusesTaxonomy.Id, Order = 300 });

                context.Taxonomies.Add(completeOrderStatusesTaxonomy);

                var enCompleteOrderStatus = EntitySeedHelper.SeedEntity(new Translation { Key = completeOrderStatusesTaxonomy.Id.ToString(), Language = LanguageConstants.English, Value = "Complete" });
                var deCompleteOrderStatus = EntitySeedHelper.SeedEntity(new Translation { Key = completeOrderStatusesTaxonomy.Id.ToString(), Language = LanguageConstants.German, Value = "Fertig" });
                var plCompleteOrderStatus = EntitySeedHelper.SeedEntity(new Translation { Key = completeOrderStatusesTaxonomy.Id.ToString(), Language = LanguageConstants.Polish, Value = "Zrealizowane" });

                context.Translations.Add(enCompleteOrderStatus);
                context.Translations.Add(deCompleteOrderStatus);
                context.Translations.Add(plCompleteOrderStatus);

                context.SaveChanges();

                // Shipped
                var shippedOrderStatusesTaxonomy = EntitySeedHelper.SeedEntity(new Taxonomy { Id = OrderStatusConstants.ShippedStatusTaxonomyId, Name = OrderStatusConstants.ShippedOrderStatusName, ParentId = orderStatusesTaxonomy.Id, Order = 400 });

                context.Taxonomies.Add(shippedOrderStatusesTaxonomy);

                var enShippedOrderStatus = EntitySeedHelper.SeedEntity(new Translation { Key = shippedOrderStatusesTaxonomy.Id.ToString(), Language = LanguageConstants.English, Value = "Shipped" });
                var deShippedOrderStatus = EntitySeedHelper.SeedEntity(new Translation { Key = shippedOrderStatusesTaxonomy.Id.ToString(), Language = LanguageConstants.German, Value = "Versand" });
                var plShippedOrderStatus = EntitySeedHelper.SeedEntity(new Translation { Key = shippedOrderStatusesTaxonomy.Id.ToString(), Language = LanguageConstants.Polish, Value = "Wysłane" });

                context.Translations.Add(enShippedOrderStatus);
                context.Translations.Add(deShippedOrderStatus);
                context.Translations.Add(plShippedOrderStatus);

                context.SaveChanges();

                // Closed
                var closedOrderStatusesTaxonomy = EntitySeedHelper.SeedEntity(new Taxonomy { Id = OrderStatusConstants.ClosedStatusTaxonomyId, Name = OrderStatusConstants.ClosedOrderStatusName, ParentId = orderStatusesTaxonomy.Id, Order = 500 });

                context.Taxonomies.Add(closedOrderStatusesTaxonomy);

                var enClosedOrderStatus = EntitySeedHelper.SeedEntity(new Translation { Key = closedOrderStatusesTaxonomy.Id.ToString(), Language = LanguageConstants.English, Value = "Closed" });
                var deClosedOrderStatus = EntitySeedHelper.SeedEntity(new Translation { Key = closedOrderStatusesTaxonomy.Id.ToString(), Language = LanguageConstants.German, Value = "Abgeschlossen" });
                var plClosedOrderStatus = EntitySeedHelper.SeedEntity(new Translation { Key = closedOrderStatusesTaxonomy.Id.ToString(), Language = LanguageConstants.Polish, Value = "Zamknięte" });

                context.Translations.Add(enClosedOrderStatus);
                context.Translations.Add(deClosedOrderStatus);
                context.Translations.Add(plClosedOrderStatus);

                context.SaveChanges();

                // Canceled
                var cancelOrderStatusesTaxonomy = EntitySeedHelper.SeedEntity(new Taxonomy { Id = OrderStatusConstants.CanceledStatusTaxonomyId, Name = OrderStatusConstants.CanceledStatusName, ParentId = orderStatusesTaxonomy.Id, Order = 600 });

                context.Taxonomies.Add(cancelOrderStatusesTaxonomy);

                var enCancelOrderStatus = EntitySeedHelper.SeedEntity(new Translation { Key = cancelOrderStatusesTaxonomy.Id.ToString(), Language = LanguageConstants.English, Value = "Canceled" });
                var deCancelOrderStatus = EntitySeedHelper.SeedEntity(new Translation { Key = cancelOrderStatusesTaxonomy.Id.ToString(), Language = LanguageConstants.German, Value = "Storniert" });
                var plCancelOrderStatus = EntitySeedHelper.SeedEntity(new Translation { Key = cancelOrderStatusesTaxonomy.Id.ToString(), Language = LanguageConstants.Polish, Value = "Anulowane" });

                context.Translations.Add(enCancelOrderStatus);
                context.Translations.Add(deCancelOrderStatus);
                context.Translations.Add(plCancelOrderStatus);

                context.SaveChanges();
            }
        }
    }
}
