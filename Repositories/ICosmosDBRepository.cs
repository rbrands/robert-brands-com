using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using robert_brands_com.Models;

namespace robert_brands_com.Repositories
{
    interface ICosmosDBRepository<T> where T : DocumentDBEntity
    {
        /// <summary>
        /// Creates or updates given document
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        Task<T> UpsertDocument(T document);
        /// <summary>
        /// Returns the document with the given id. If the document does not exist null is returned.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetDocument(string id);
        /// <summary>
        /// Returns the document by the given "logical" key. The logical key is used to build the document id
        /// according to the pattern tenant-type-key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetDocumentByKey(string key);
        Task<T> CreateDocument(T document);
        Task<IEnumerable<T>> GetDocuments(Expression<Func<T, bool>> predicate, int maxItemCount = -1);
        Task<PagedResult<T>> GetPagedDocumentsDescending<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> keySelector, int maxItemCount, string pagingToken);
        Task<IEnumerable<T>> GetDocuments();
        Task DeleteDocumentAsync(string id);
    }
}
}
