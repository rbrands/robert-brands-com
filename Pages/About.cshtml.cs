using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using robert_brands_com.Models;
using robert_brands_com.Repositories;

namespace robert_brands_com.Pages
{
    [AllowAnonymous]
    public class AboutModel : LoggedPageModel
    {
        public AboutModel(IActivityLog activityLog) : base(activityLog, "About")
        {

        }
        public async void OnGet()
        {
            await this.LogGetActivity();
        }
    }
}