using MongoDB.Driver;
using System.Linq.Expressions;

namespace MicroServices.Common.MongoDB
{
    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {
        //private const string collectionName = "items";
        private readonly IMongoCollection<T> dbCollection;
        private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

        public MongoRepository(IMongoDatabase mongoDatabase, string collectionName)
        {
            //var mongoClient = new MongoClient("mongodb://localhost:27017");
            //var database = mongoClient.GetDatabase("Catalog");
            dbCollection = mongoDatabase.GetCollection<T>(collectionName);
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).ToListAsync();
        }

        public async Task<T> GetAsync(Guid id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(entity => entity.Id, id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            await dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            FilterDefinition<T> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);
        }

        public async Task RemoveAsync(Guid Id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, Id);
            await dbCollection.DeleteOneAsync(filter);
        }

    }
}
