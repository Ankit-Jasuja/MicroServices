using MicroServices.Common;
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

        public ItemsController(IRepository<InventoryItem> repository)
        {
            _inventoryItemRepository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userID)
        {
            if (userID == Guid.Empty)
            {
                return BadRequest();
            }

            var items = (await _inventoryItemRepository.GetAllAsync(item => item.UserId == userID))
               .Select(item => item.AsDto());

            return Ok(items);
        }

        [HttpGet]
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
