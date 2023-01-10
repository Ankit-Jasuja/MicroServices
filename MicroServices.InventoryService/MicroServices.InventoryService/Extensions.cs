using MicroServices.InventoryService.Entities;
using static MicroServices.InventoryService.Dto;

namespace MicroServices.InventoryService
{
    public static class Extensions
    {
        public static InventoryItemDto AsDto(this InventoryItem inventoryItem, string name, string description)
        {
            return new InventoryItemDto(inventoryItem.CatalogItemID,name, description,
                inventoryItem.Quantity, inventoryItem.AcquiredDate);
        }
    }
}
