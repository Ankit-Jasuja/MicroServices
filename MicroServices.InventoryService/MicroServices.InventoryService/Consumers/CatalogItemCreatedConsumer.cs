using MassTransit;
using MicroServices.Common;
using MicroServices.InventoryService.Entities;
using static MicroServices.Catalog.Contract.Contracts;

namespace MicroServices.InventoryService.Consumers
{
    public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
    {
        private readonly IRepository<CatalogItem> _repository;

        public CatalogItemCreatedConsumer(IRepository<CatalogItem> repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemCreated> context)
        {
            var message = context.Message;
            var item = await _repository.GetAsync(message.CtalogItemId);
            if (item is not null)
            {
                return;
            }

            item = new CatalogItem
            {
                Id = message.CtalogItemId,
                Name = message.Name,
                Description = message.Description
            };

            await _repository.CreateAsync(item);
        }
    }
}
