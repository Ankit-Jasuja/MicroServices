﻿using MassTransit;
using MicroServices.Common;
using MicroServices.InventoryService.Entities;
using static MicroServices.Catalog.Contract.Contracts;

namespace MicroServices.InventoryService.Consumers
{
    public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
    {
        private readonly IRepository<CatalogItem> _repository;

        public CatalogItemUpdatedConsumer(IRepository<CatalogItem> repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
        {
            var message = context.Message;
            var item = await _repository.GetAsync(message.CtalogItemId);
            if (item is null)
            {
                item = new CatalogItem
                {
                    Id = message.CtalogItemId,
                    Name = message.Name,
                    Description = message.Description
                };

                await _repository.CreateAsync(item);
            }
            else
            {
                item.Name = message.Name;
                item.Description = message.Description;

                await _repository.UpdateAsync(item);
            }

        }
    }
}
