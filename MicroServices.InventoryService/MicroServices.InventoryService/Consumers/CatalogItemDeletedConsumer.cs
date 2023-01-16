using MassTransit;
using MicroServices.Common;
using MicroServices.InventoryService.Entities;
using static MicroServices.Catalog.Contract.Contracts;

namespace MicroServices.InventoryService.Consumers
{
    public class CatalogItemDeletedConsumer : IConsumer<CatalogItemDeleted>
    {
        private readonly IRepository<CatalogItem> _repository;

        public CatalogItemDeletedConsumer(IRepository<CatalogItem> repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
        {
            var message = context.Message;
            var item = await _repository.GetAsync(message.CtalogItemId);
            if (item is null)
            {
                return;
            }
            await _repository.RemoveAsync(message.CtalogItemId);
        }
    }
}
