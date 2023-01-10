using MicroServices.InventoryService.Entities;
using static MicroServices.InventoryService.Dto;

namespace MicroServices.InventoryService
{
    public static class Extensions
    {
        public static InventoryItemDto AsDto(this InventoryItem inventoryItem)
        {
            return new InventoryItemDto(inventoryItem.CatalogItemID, inventoryItem.Quantity, inventoryItem.AcquiredDate);
        }
    }
}
