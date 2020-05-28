using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using robert_brands_com.Models;

namespace robert_brands_com.Repositories
{
    public class PagedResult<T> where T : DocumentDBEntity
    {
        public string ContinuationToken { get; set; }
        public List<T> Result { get; set; }

    }
}
