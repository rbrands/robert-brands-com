using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace robert_brands_com.Pages
{
    [AllowAnonymous]
    public class AboutModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}