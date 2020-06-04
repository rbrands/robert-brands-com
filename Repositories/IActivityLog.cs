using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace robert_brands_com.Repositories
{
    /// <summary>
    /// Interface to log activities
    /// </summary>
    public interface IActivityLog
    {
        Task LogActivity(string application, string category, string user, string messageTag, string message, string clientInfo);
    }
}
