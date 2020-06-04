using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using robert_brands_com.Models;
using robert_brands_com.Repositories;

namespace robert_brands_com.Pages
{
    public class LoggedPageModel : PageModel
    {
        const string appName = "robertbrands";
        readonly string category;
        private readonly IActivityLog _activityLogger;

        public LoggedPageModel(IActivityLog activityLog, string pageCategory)
        {
            _activityLogger = activityLog;
            category = pageCategory;
        }
        public async Task LogGetActivity()
        {
            await _activityLogger.LogActivity(appName, category, this.User.Identity.Name, this.GetType().Name, this.GetType().Name + " called", this.Request.Headers["User-Agent"]);
        }
        public async Task LogActivity(string message)
        {
            await _activityLogger.LogActivity(appName, category, this.User.Identity.Name, this.GetType().Name, message, this.Request.Headers["User-Agent"]);
        }
    }
}

