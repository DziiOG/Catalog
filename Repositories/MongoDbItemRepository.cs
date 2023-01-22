using Catalog.Interfaces.Repositories;
using System.Threading.Tasks;
using Catalog.Models;
using MongoDB.Driver;

namespace Catalog.Repositories
{
    public class MongoDbItemRepository : IItemRepository
    {
        private readonly IMongoCollection<Item> itemsCollections;
        private const string databaseName = "catalog";
        private const string collectionName = "items";

        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        public MongoDbItemRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            itemsCollections = database.GetCollection<Item>(collectionName);
        }

        public async Task CreateItemAsync(Item item)
        {
            await itemsCollections.InsertOneAsync(item);
        }

        public async Task DeleteItemAsync(Guid id)
        {
            FilterDefinition<Item>? filter = filterBuilder.Eq(item => item.Id, id);
            await itemsCollections.DeleteOneAsync(filter);
        }

        public async Task<Item>? GetItemAsync(Guid id)
        {
            FilterDefinition<Item>? filter = filterBuilder.Eq(item => item.Id, id);
            return await itemsCollections.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await itemsCollections.Find(new MongoDB.Bson.BsonDocument()).ToListAsync();
        }

        public async Task UpdateItemAsync(Item item)
        {
            FilterDefinition<Item>? filter = filterBuilder.Eq(
                existingItem => existingItem.Id,
                item.Id
            );
            await itemsCollections.ReplaceOneAsync(filter, item);
        }
    }
}
