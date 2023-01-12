using MicroServices.Common.MongoDB;
using MicroServices.InventoryService.Clients;
using MicroServices.InventoryService.Entities;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMongo()
    .AddMongoRepo<InventoryItem>("inventoryitems");

builder.Services.AddHttpClient<CatalogClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7105");
})
.AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1)); //whenever we invoke anything under localhost:7105,
                                                                //we are going to wait for one sec before giving up

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
