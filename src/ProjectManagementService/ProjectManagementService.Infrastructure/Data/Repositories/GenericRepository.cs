using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.Configuration;
using ProjectManagementService.Domain.Entities;
using System.Linq.Expressions;

namespace HiringService.Infrastructure.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : EntityWithId
{
    private readonly IMongoCollection<T> _collection;

    public GenericRepository(IMongoClient mongoClient,
        IOptions<MongoDBConfiguration> DBConfigurationOptions)
    {
        var DBConfiguration = DBConfigurationOptions.Value;
        var collectionConfigName = typeof(T).Name + "CollectionName";

        var databaseName = DBConfiguration.DatabaseName;
        var collectionName = DBConfiguration.Collections[collectionConfigName];

        var database = mongoClient.GetDatabase(databaseName);
        _collection = database.GetCollection<T>(collectionName);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return (await _collection.FindAsync(_ => true)).ToList();
    }

    public async Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate)
    {
        return (await _collection.FindAsync(predicate)).ToList();
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        var filter = new BsonDocument { { "_id", new BsonDocument("$eq", new ObjectId(id)) } };

        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate)
    {
        return await _collection.Find(predicate).FirstOrDefaultAsync();
    }

    public async Task<string> AddOneAsync(T item)
    {
        await _collection.InsertOneAsync(item);

        return item.Id;
    }

    public async Task AddManyAsync(List<T> items)
    {
        await _collection.InsertManyAsync(items);
    }

    public async Task RemoveAsync(string id)
    {
        var filter = new BsonDocument { { "_id", new BsonDocument("$eq", new ObjectId(id)) } };

        await _collection.DeleteOneAsync(filter);
    }

    public async Task UpdateAsync(BsonDocument update, Expression<Func<T, bool>> predicate)
    {
        await _collection.UpdateOneAsync(predicate, update);
    }

    public async Task UpdatePropertyAsync(string id, string propertyName, BsonValue value)
    {
        var update = new BsonDocument("$set", new BsonDocument(propertyName, value));

        await UpdateAsync(update, task => task.Id == id);
    }
}
