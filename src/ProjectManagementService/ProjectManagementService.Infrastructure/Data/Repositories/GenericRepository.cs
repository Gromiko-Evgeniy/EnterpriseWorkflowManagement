using HiringService.Application.Abstractions;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace HiringService.Infrastructure.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly IMongoCollection<T> _collection;

    public GenericRepository(IConfiguration configuration, IMongoClient mongoClient, string collectionNameConfigName)
    {
        var databaseName = configuration["DBConnectionSettings:DatabaseName"];
        var collectionName = configuration["DBConnectionSettings:Collections:" + collectionNameConfigName];

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

        return await _collection.Find(filter).FirstAsync();
    }

    public async Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate)
    {
        return await _collection.Find(predicate).FirstAsync();
    }

    public async Task<T> AddAsync(T item)
    {
        await _collection.InsertOneAsync(item);

        return item;
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