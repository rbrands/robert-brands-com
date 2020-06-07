using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using robert_brands_com.Models;
using robert_brands_com.Repositories;


namespace robert_brands_com.Pages.Fotos
{
    [Authorize(Roles = KnownRoles.AllRolesList)]
    public class AlbumModel : PageModel
    {
        [BindProperty]
        public string Search { get; set; }

        private ICosmosDBRepository<CommentedLinkItem> repository;
        private IActivityLog activityLog;
        public IEnumerable<CommentedLinkItem> CommentedLinks { get; private set; }
        public AlbumModel(ICosmosDBRepository<CommentedLinkItem> repositoryService, IActivityLog activityLogRepository)
        {
            this.repository = repositoryService;
            this.activityLog = activityLogRepository;
        }

        public async Task OnGet(string category = null)
        {
            if (String.IsNullOrEmpty(category))
            {
                IEnumerable<CommentedLinkItem> documents = await repository.GetDocuments();
                CommentedLinks = documents.OrderByDescending(d => d.Date);
            }
            else
            {
                IEnumerable<CommentedLinkItem> documents = await repository.GetDocuments(d => d.Category == category);
                CommentedLinks = documents.OrderByDescending(d => d.Date);
            }
        }
        public async Task<IActionResult> OnGetSearchAsync(string searchExpression)
        {
            if (String.IsNullOrEmpty(searchExpression))
            {
                return new NotFoundResult();
            }
            IEnumerable<CommentedLinkItem> documents = await repository.GetDocuments(d => d.Title.IndexOf(searchExpression, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                                                                          d.Description.IndexOf(searchExpression, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                                                                          d.Category.IndexOf(searchExpression, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                                                                          d.ListName.IndexOf(searchExpression, StringComparison.OrdinalIgnoreCase) >= 0);
            CommentedLinks = documents.OrderByDescending(d => d.Date);
            return Page();
        }
        // only on page level [Authorize(KnownRoles.Admin)]
        public async Task<IActionResult> OnGetDeleteLinkAsync(string documentid)
        {
            if (String.IsNullOrEmpty(documentid))
            {
                return new NotFoundResult();
            }
            await repository.DeleteDocumentAsync(documentid);
            ViewData["Message"] = "Eintrag gelöscht";

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostSearchAsync()
        {
            if (String.IsNullOrEmpty(Search))
            {
                return RedirectToPage();
            }
            string searchLowercase = Search.ToLower();
            IEnumerable<CommentedLinkItem> documents = await repository.GetDocuments(d => d.Title.ToLower().Contains(searchLowercase) ||
                                                                                     d.Description.ToLower().Contains(searchLowercase) ||
                                                                                     d.Category.ToLower().Contains(searchLowercase) ||
                                                                                     d.ListName.ToLower().Contains(searchLowercase));
            CommentedLinks = documents.OrderByDescending(d => d.Date);
            return Page();
        }
    }
}