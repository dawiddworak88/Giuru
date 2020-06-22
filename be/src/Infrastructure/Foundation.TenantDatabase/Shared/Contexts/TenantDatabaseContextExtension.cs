using Foundation.TenantDatabase.Shared.Seeds;

namespace Foundation.TenantDatabase.Shared.Contexts
{
    public static class TenantDatabaseContextExtension
    {
        public static void EnsureSeeded(this TenantDatabaseContext context)
        {
            context.EnsureEntityTypesSeeded();
        }
    }
}
