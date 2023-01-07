using CatalogService.Entities;

namespace CatalogService.Repositories
{
    public interface IItemsRepository
    {
        Task CreateAsync(Item entity);
        Task<IReadOnlyCollection<Item>> GetAllAsync();
        Task<Item> GetAsync(Guid id);
        Task RemoveAsync(Guid Id);
        Task UpdateAsync(Item entity);
    }
}