using robert_brands_com.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;

namespace robert_brands_com.Repositories
{
    public class DbConfig
    {
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
        public string EndPointUrl { get; set; }
        public string AuthorizationKey { get; set; }
        public bool CreateIfNotExists { get; set; }
    }
    /// <summary>
    /// Access to Cosmos DB is provided by this class that should created in Startup. All objects stored in the database are inherited from DocumentDBEntity
    /// Create for every type to be accessed a repository in Startup.
    /// </summary>
    /// <typeparam name="T">Type that is handled by this repository</typeparam>
    public class CosmosDBRepository<T> : ICosmosDBRepository<T> where T : DocumentDBEntity, new()
    {
        private DbConfig _config;
        private CosmosClient __cosmosClient;

        public CosmosDBRepository(DbConfig dbConfig)
        {
            _config = dbConfig;
            __cosmosClient = this.CreateCosmosClient();
        }

        private CosmosClient CreateCosmosClient()
        {
            CosmosClient client = new CosmosClient(_config.EndPointUrl, _config.AuthorizationKey);
            if (_config.CreateIfNotExists)
            {
                // Check if database exists and create one if not
                Database database = client.CreateDatabaseIfNotExistsAsync(_config.DatabaseName).Result;

                // Create new container if not already exists
                database.CreateContainerIfNotExistsAsync(_config.CollectionName, "/partionKey");
            }
            return client;
        }
        public async Task<T> CreateDocument(T document)
        {
            if (null == document)
            {
                throw new ArgumentNullException("document");
            }
            document.SetUniqueKey();
            return await __cosmosClient.GetDatabase(_config.DatabaseName)
                                       .GetContainer(_config.CollectionName)
                                       .CreateItemAsync<T>(document, new PartitionKey(document.PartitionKey));
        }

        public Task DeleteDocumentAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetDocument(string id)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetDocumentByKey(string key)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetDocuments(Expression<Func<T, bool>> predicate, int maxItemCount = -1)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetDocuments()
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<T>> GetPagedDocumentsDescending<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> keySelector, int maxItemCount, string pagingToken)
        {
            throw new NotImplementedException();
        }

        public Task<T> UpsertDocument(T document)
        {
            throw new NotImplementedException();
        }
    }
}
