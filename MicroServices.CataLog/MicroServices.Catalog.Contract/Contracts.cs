namespace MicroServices.Catalog.Contract
{
    public class Contracts
    {
        public record CatalogItemCreated(Guid CtalogItemId,string Name,string Description);
        public record CatalogItemUpdated(Guid CtalogItemId,string Name,string Description);
        public record CatalogItemDeleted(Guid CtalogItemId);
    }
}
