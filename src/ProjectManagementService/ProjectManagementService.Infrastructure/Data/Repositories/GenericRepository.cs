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
        try
        {
            return (await _collection.FindAsync(predicate)).ToList();
        }
        catch (Exception ex)
        {
            return new List<T>();
        }
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        var filter = new BsonDocument { { "_id", new BsonDocument("$eq", new ObjectId(id)) } };

        return await _collection.Find(filter).FirstAsync();
    }

    public async Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate)
    {
        try
        {
            return await _collection.Find(predicate).FirstAsync();
        }
        catch
        {
            return null;
        }
    }

    public async Task<string> AddAsync(T item)
    {
        await _collection.InsertOneAsync(item);

        return item.Id;
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
}
