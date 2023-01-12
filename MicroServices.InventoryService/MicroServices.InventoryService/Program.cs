using MicroServices.Common.MongoDB;
using MicroServices.InventoryService.Clients;
using MicroServices.InventoryService.Entities;
using Polly;
using Polly.Timeout;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMongo()
    .AddMongoRepo<InventoryItem>("inventoryitems");

Random random = new();

builder.Services.AddHttpClient<CatalogClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7105");
})
.AddTransientHttpErrorPolicy(Pbuilder => Pbuilder.Or<TimeoutRejectedException>().WaitAndRetryAsync(
    5,
    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
    + TimeSpan.FromMilliseconds(random.Next(0,1000)),
    onRetry: (outcome, timespan, retryAttempt) =>
    {
        // do not do in production
        var serviceProvider = builder.Services?.BuildServiceProvider();
        serviceProvider.GetService<ILogger<CatalogClient>>()?
        .LogWarning($"delaying for {timespan.TotalSeconds} seconds,then making retry {retryAttempt}");
    }
    ))
.AddTransientHttpErrorPolicy(Pbuilder => Pbuilder.Or<TimeoutRejectedException>().CircuitBreakerAsync(
    3,
    TimeSpan.FromSeconds(15),
    onBreak: (outcome, timespan) =>
    {
        // do not do in production
        var serviceProvider = builder.Services?.BuildServiceProvider();
        serviceProvider.GetService<ILogger<CatalogClient>>()?
        .LogWarning($"opening the circuit for {timespan.TotalSeconds} seconds");
    },
    onReset: () =>
    {
        // do not do in production
        var serviceProvider = builder.Services?.BuildServiceProvider();
        serviceProvider.GetService<ILogger<CatalogClient>>()?
        .LogWarning($"closing the circut....");
    }

))
.AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1)); //whenever we invoke anything under localhost:7105,
                                                                //we are going to wait for one sec before giving up

builder.Services.AddControllers();
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
