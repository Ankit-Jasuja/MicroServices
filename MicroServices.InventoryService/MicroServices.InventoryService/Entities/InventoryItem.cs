using MicroServices.Common;

namespace MicroServices.InventoryService.Entities
{
    public class InventoryItem : IEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid CatalogItemID { get; set; }

        public int Quantity { get; set; }    

        public DateTimeOffset AcquiredDate { get; set; }    
    }
}

