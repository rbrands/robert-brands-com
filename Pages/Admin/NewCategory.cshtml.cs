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
    public class NewCategoryModel : PageModel
    {
        private ICosmosDBRepository<ListCategory> repository;
        [BindProperty]
        public ListCategory Category { get; set; }

        public NewCategoryModel(ICosmosDBRepository<ListCategory> repository)
        {
            this.repository = repository;
        }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnGetChangeCategoryAsync(string documentid)
        {
            if (String.IsNullOrEmpty(documentid))
            {
                return new NotFoundResult();
            }
            Category = await repository.GetDocument(documentid);
            if (null == Category)
            {
                return new NotFoundResult();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                IEnumerable<ListCategory> list = await repository.GetDocuments(s => s.ListName == Category.ListName && s.Category == Category.Category);
                ListCategory existingOne = list.FirstOrDefault();
                if (null != existingOne)
                {
                    Category.Id = existingOne.Id;
                }
                await repository.UpsertDocument(Category);
                ViewData["Message"] = "Kategorie gespeichert.";
                return RedirectToPage("Kategorien");
            }
            else
            {
                ViewData["Message"] = "Schiefgegangen";
                return Page();
            }
        }
    }
}