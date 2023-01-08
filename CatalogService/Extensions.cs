using Microservices.CatalogService.Entities;
using static Microservices.CatalogService.Dto;

namespace Microservices.CatalogService
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
        }
    }
}
