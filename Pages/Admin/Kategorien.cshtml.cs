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
    public class KategorienModel : PageModel
    {
        private ICosmosDBRepository<ListCategory> repository;
        public IEnumerable<ListCategory> CategoryList { get; private set; }
        public KategorienModel(ICosmosDBRepository<ListCategory> repository)
        {
            this.repository = repository;
        }
        public async Task OnGetAsync()
        {
            CategoryList = await repository.GetDocuments();
        }
        public async Task<IActionResult> OnGetDeleteAsync(string documentid)
        {
            await repository.DeleteDocumentAsync(documentid);
            ViewData["Message"] = "Eintrag gelöscht";

            return RedirectToPage();
        }
    }
}