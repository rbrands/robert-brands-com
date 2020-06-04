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
    public class NewModel : PageModel
    {
        private ICosmosDBRepository<Shortcut> repository;
        [BindProperty]
        public Shortcut NewShortcut { get; set; }

        public NewModel(ICosmosDBRepository<Shortcut> repositoryService)
        {
            repository = repositoryService;
        }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await repository.CreateDocument(NewShortcut);
                ViewData["Message"] = "Shortcut gespeichert.";
                return RedirectToPage("./Index");
            }
            else
            {
                ViewData["Message"] = "Schiefgegangen";
                return Page();
            }
        }
        public async Task<IActionResult> OnGetChangeShortcutAsync(string category, string nickname)
        {

            IEnumerable<Shortcut> list = await repository.GetDocuments(s => s.Category == category && s.Nickname == nickname);
            NewShortcut = list.FirstOrDefault();
            return Page();
        }

    }
}