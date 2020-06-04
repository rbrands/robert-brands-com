using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using robert_brands_com.Models;
using Microsoft.Extensions.Configuration;


namespace robert_brands_com.Repositories
{
    public class ActivityLogDBRepository : IActivityLog
    {
        private CosmosDBRepository<ActivityLogItem> _documentRepository;
        private int ttl;
        public ActivityLogDBRepository(IConfiguration configuration, DbConfig dbConfig)
        {
            _documentRepository = new CosmosDBRepository<ActivityLogItem>(dbConfig);
            ttl = configuration.GetValue<int>("ActivityLogTTL");
        }
        public async Task LogActivity(string application, string category, string user, string messageTag, string message, string clientInfo)
        {
            ActivityLogItem logActivity = new ActivityLogItem();
            logActivity.Category = category;
            logActivity.User = user;
            logActivity.MessageTag = messageTag;
            logActivity.Message = message;
            logActivity.ClientInfo = clientInfo;
            logActivity.TimeToLive = ttl;
            logActivity.TimeStamp = DateTime.UtcNow;

            await _documentRepository.UpsertDocument(logActivity);

            return;
        }
    }
}
