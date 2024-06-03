using Microsoft.EntityFrameworkCore;

namespace Global.Api.Infrastructure.Entities.Settings
{
    [Keyless]
    public class Setting
    {
        public bool ExternalCompletionDates { get; set; }
    }
}
