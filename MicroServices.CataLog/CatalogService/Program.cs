using Microservices.CatalogService.Entities;
using MicroServices.Common.MongoDB;
using MicroServices.Common.Settings;
using MassTransit;
using Microservices.CatalogService.Settings;
using MassTransit.Definition;

var builder = WebApplication.CreateBuilder(args);
ServiceSettings? serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

builder.Services.AddMongo()
    .AddMongoRepo<Item>("items");

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, configurator) =>
    {
        var rabbitMqSettings = builder?.Configuration?.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
        configurator.Host(rabbitMqSettings?.Host);
        configurator.ConfigureEndpoints(context,new KebabCaseEndpointNameFormatter(serviceSettings?.ServiceName,false));
    });
});

builder.Services.AddMassTransitHostedService();

builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
