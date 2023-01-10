using MicroServices.Common;
using MicroServices.InventoryService.Clients;
using MicroServices.InventoryService.Entities;
using Microsoft.AspNetCore.Mvc;
using static MicroServices.InventoryService.Dto;

namespace MicroServices.InventoryService.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<InventoryItem> _inventoryItemRepository;
        private readonly CatalogClient _catalogClient;

        public ItemsController(IRepository<InventoryItem> repository, CatalogClient catalogClient)
        {
            _inventoryItemRepository = repository;
            _catalogClient = catalogClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userID)
        {
            if (userID == Guid.Empty)
            {
                return BadRequest();
            }

            var allCataLogItems = await _catalogClient.GetCatalogItemsAsync();

            var inventoryItems = await _inventoryItemRepository.GetAllAsync(item => item.UserId == userID);

            var inventoryItemDtos = inventoryItems.Select(item =>
            {
                var catalogItem = allCataLogItems.Single(z => z.Id == item.CatalogItemID);
                return item.AsDto(catalogItem.Name, catalogItem.Description);
            });

            return Ok(inventoryItemDtos);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemDto grantItemDto)
        {
            var existingInventoryItem = await _inventoryItemRepository.GetAsync(
                z => z.UserId == grantItemDto.UserId && z.CatalogItemID == grantItemDto.CatalogItemId);

            if (existingInventoryItem is null)
            {
                var item = new InventoryItem
                {
                    UserId = grantItemDto.UserId,
                    CatalogItemID = grantItemDto.CatalogItemId,
                    Quantity = grantItemDto.quantity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };
                await _inventoryItemRepository.CreateAsync(item);
            }
            else
            {
                existingInventoryItem.Quantity += grantItemDto.quantity;
                await _inventoryItemRepository.UpdateAsync(existingInventoryItem);
            }

            return Ok();
        }
    }
}
