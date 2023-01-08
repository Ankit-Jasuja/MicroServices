using CatalogService.Entities;
using CatalogService.Repositories;
using Microsoft.AspNetCore.Mvc;
using static CatalogService.Dto;

namespace CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> _itemsRepository;

        public ItemsController(IRepository<Item> itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            Console.WriteLine("getting items");
            var items = (await _itemsRepository.GetAllAsync()).Select(z => z.AsDto());
            return items;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid Id)
        {
            Console.WriteLine("getting item by Id.......");
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
            Console.WriteLine("creating item.......");
            var item = new Item
            {
                Id = Guid.NewGuid(),
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.Now
            };
            await _itemsRepository.CreateAsync(item);
            return CreatedAtAction(nameof(GetByIdAsync), new { item.Id }, item);
        }

        [HttpPut("Id")]
        public async Task<IActionResult> Put(Guid Id, UpdateItemDto updateItemDto)
        {
            Console.WriteLine("updating item.......");
            var existingItem = await _itemsRepository.GetAsync(Id);
            if (existingItem is null)
            {
                return NotFound();
            }
            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = updateItemDto.Price;

            await _itemsRepository.UpdateAsync(existingItem);

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
            return NoContent();
        }
    }
}
