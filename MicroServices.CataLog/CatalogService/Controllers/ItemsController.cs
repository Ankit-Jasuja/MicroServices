using MassTransit;
using Microservices.CatalogService.Entities;
using MicroServices.Common;
using Microsoft.AspNetCore.Mvc;
using static Microservices.CatalogService.Dto;
using static MicroServices.Catalog.Contract.Contracts;

namespace Microservices.CatalogService.Controllers
{
    [Route("items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> _itemsRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public ItemsController(IRepository<Item> itemsRepository, IPublishEndpoint publishEndpoint)
        {
            _itemsRepository = itemsRepository;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
        {
            var items = (await _itemsRepository.GetAllAsync()).Select(z => z.AsDto());
            return Ok(items);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid Id)
        {
            var item = await _itemsRepository.GetAsync(Id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItem(CreateItemDto createItemDto)
        {
            var item = new Item
            {
                Id = Guid.NewGuid(),
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.Now
            };
            await _itemsRepository.CreateAsync(item);
            await _publishEndpoint.Publish( new CatalogItemCreated(item.Id, item.Name, item.Description));
            return CreatedAtAction(nameof(GetByIdAsync), new { item.Id }, item);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(Guid Id, UpdateItemDto updateItemDto)
        {
            var existingItem = await _itemsRepository.GetAsync(Id);
            if (existingItem is null)
            {
                return NotFound();
            }
            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = updateItemDto.Price;

            await _itemsRepository.UpdateAsync(existingItem);
            await _publishEndpoint.Publish(new CatalogItemUpdated(existingItem.Id, existingItem.Name, 
                existingItem.Description));

            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var existingItem = await _itemsRepository.GetAsync(Id);
            if (existingItem is null)
            {
                return NotFound();
            }
            await _itemsRepository.RemoveAsync(existingItem.Id);
            await _publishEndpoint.Publish(new CatalogItemDeleted(Id));
            return NoContent();
        }
    }
}
