using System.Threading.Tasks;

namespace Foundation.EventBus.Abstractions
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
