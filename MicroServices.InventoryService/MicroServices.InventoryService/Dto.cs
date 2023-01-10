namespace MicroServices.InventoryService
{
    public class Dto
    {
        //to grant item to user with UserId
        public record GrantItemDto(Guid UserId, Guid CatalogItemId, int quantity);

        //to return series of items that a user has in his inventory
        public record InventoryItemDto(Guid CatalogItemId, string Name, string Description, int quantity,
            DateTimeOffset AcquiredDate);

        public record CatalogItemDto(Guid Id, string Name, string Description);
    }
}
