using Foundation.GenericRepository.Entities;

namespace Foundation.GenericRepository.Services
{
    public interface IEntityService
    {
        T EnrichEntity<T>(T entity) where T : Entity;
    }
}
