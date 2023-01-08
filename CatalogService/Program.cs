using CatalogService.Entities;
using CatalogService.Repositories;
using CatalogService.Settings;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
ServiceSettings serviceSettings;

// Add services to the container.

builder.Services.AddMongo()
    .AddMongoRepo<Item>("items");

builder.Services.AddSingleton<IRepository<Item>>(sp =>
{
    var database = sp.GetService<IMongoDatabase>();
    return new MongoRepository<Item>(database!, "items");
});

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
