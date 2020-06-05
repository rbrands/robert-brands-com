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

namespace robert_brands_com.Pages.Shortcuts
{
    [Authorize(Roles = KnownRoles.Admin)]
    public class IndexModel : PageModel
    {
        private ICosmosDBRepository<Shortcut> repository;
        public IEnumerable<Shortcut> Shortcuts { get; private set; }

        public IndexModel(ICosmosDBRepository<Shortcut> repositoryService)
        {
            repository = repositoryService;
        }
        public async Task OnGet()
        {
            Shortcuts = await repository.GetDocuments();
        }
        public async Task<IActionResult> OnGetDeleteAsync(string category, string nickname)
        {
            Shortcut shortcut = (await repository.GetDocuments(s => s.Category == category && s.Nickname == nickname)).FirstOrDefault();
            if (null != shortcut)
            {
                await repository.DeleteDocumentAsync(shortcut.Id);
            }
            ViewData["Message"] = "Eintrag gelöscht";
            return RedirectToPage();
        }
    }
}