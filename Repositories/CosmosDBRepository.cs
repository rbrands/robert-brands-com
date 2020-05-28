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
    public class CosmosDBRepository<T> : ICosmosDBRepository<T> where T : DocumentDBEntity, new()
    {
        public Task<T> CreateDocument(T document)
        {
            throw new NotImplementedException();
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
