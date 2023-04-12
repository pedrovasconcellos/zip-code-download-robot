using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using ZipeCodeConsoleCore.Models;

namespace ZipeCodeConsoleCore.Repository
{
    public class MongoDBRepository
    {
        private readonly ILogger<MongoDBRepository> _logger;
        private readonly string _connectionString;

        public MongoDBRepository(ILogger<MongoDBRepository> logger, string connectionString)
        {
            this._connectionString = connectionString;
            this._logger = logger;
        }

        private IMongoDatabase GetMongoDatabase()
        {
            var mongoUrl = MongoUrl.Create(this._connectionString);
            var settings = MongoClientSettings.FromUrl(mongoUrl);
            var client = new MongoClient(settings);
            return client.GetDatabase(mongoUrl.DatabaseName);
        }

        public bool ZipeCodeExist<T>(string zipeCode) where T : ZipeCodeBase
        {
            try
            {
                var database = this.GetMongoDatabase();
                var collectionName = typeof(T).Name;
                var collection = database
                    .GetCollection<ZipeCodeBase>(collectionName);

                return collection
                    .AsQueryable<ZipeCodeBase>()
                    .Any(x => x.cep == zipeCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error: {ex.Message}");
                Thread.Sleep(600000);
                return false;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : ZipeCodeBase
        {
            try
            {
                var database = this.GetMongoDatabase();
                var collectionName = typeof(T).Name;
                var collection = database
                    .GetCollection<T>(collectionName);

                var result = (await collection.AsQueryable().ToListAsync());
                return result.OrderBy(x => x.cep);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error: {ex.Message}");
                return new List<T>();
            }
        }
        public async Task<IEnumerable<ZipeCodeNumber>> GetAllAsyncNumber()
        {
            try
            {
                var database = this.GetMongoDatabase();
                var collectionName = typeof(ZipeCodeNumber).Name;
                var collection = database
                    .GetCollection<ZipeCodeNumber>(collectionName);

                var result = (await collection.AsQueryable().ToListAsync());
                return result.OrderBy(x => x.cep);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error: {ex.Message}");
                return new List<ZipeCodeNumber>();
            }
        }

        public async Task<bool> InsertOneAsync<T>(T entity) where T : ZipeCodeBase
        {
            try
            {
                var database = this.GetMongoDatabase();
                var collectionName = typeof(T).Name;
                var collection = database
                    .GetCollection<T>(collectionName);

                await collection.InsertOneAsync(entity);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error: {ex.Message}");
                return false;
            }
        }
    }
}

