using CatalogService.Entities;

namespace CatalogService.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        Task CreateAsync(T entity);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<T> GetAsync(Guid id);
        Task RemoveAsync(Guid Id);
        Task UpdateAsync(T entity);
    }
}