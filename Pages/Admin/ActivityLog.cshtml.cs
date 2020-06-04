using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using robert_brands_com.Models;
using robert_brands_com.Repositories;


namespace robert_brands_com.Pages.Admin
{
    [Authorize(Roles = KnownRoles.Admin)]
    public class ActivityLogModel : PageModel
    {
        private ICosmosDBRepository<ActivityLogItem> _activityLog;
        public IEnumerable<ActivityLogItem> Activities { get; private set; }

        public ActivityLogModel(ICosmosDBRepository<ActivityLogItem> activityLog)
        {
            _activityLog = activityLog; 
        }
        public async Task OnGetAsync()
        {
            Activities = (await _activityLog.GetPagedDocumentsDescending(d => true, a => a.TimeStamp, 150, null)).Result;
        }
        public async Task OnGetNoRobotsAsync()
        {
            Activities = (await _activityLog.GetPagedDocumentsDescending(d => !d.ClientInfo.Contains("bot") && !d.ClientInfo.Contains("Bot") && !d.ClientInfo.Contains("crawler") && !d.ClientInfo.Contains("Crawler"), a => a.TimeStamp, 150, null)).Result;
        }
    }
}