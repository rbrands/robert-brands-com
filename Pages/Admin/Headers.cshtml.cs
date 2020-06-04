using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace robert_brands_com.Pages.Admin
{
    [Authorize(Roles = KnownRoles.Admin)]
    public class HeadersModel : PageModel
    {
        [BindProperty]
        public IConfiguration Configuration { get; set; }
        public HeadersModel(IConfiguration configuration)
        {
            Configuration = configuration;

        }
        public void OnGet()
        {

        }
    }
}