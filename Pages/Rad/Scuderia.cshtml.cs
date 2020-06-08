using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using robert_brands_com.Models;
using robert_brands_com.Repositories;

namespace robert_brands_com.Pages.Rad
{
    [AllowAnonymous]
    public class ScuderiaModel : LoggedPageModel
    {
        private const string listName = "Scuderia";
        private ICosmosDBRepository<CommentedLinkItem> repository;
        private IActivityLog activityLog;
        public IEnumerable<CommentedLinkItem> CommentedLinks { get; private set; }
        public ScuderiaModel(ICosmosDBRepository<CommentedLinkItem> repositoryService, IActivityLog activityLogRepository) : base(activityLogRepository, "Rad")
        {
            this.repository = repositoryService;
            this.activityLog = activityLogRepository;
        }
        public async Task OnGet(string category = null)
        {
            await this.LogGetActivity();
            if (String.IsNullOrEmpty(category))
            {
                IEnumerable<CommentedLinkItem> documents = await repository.GetDocuments(d => d.ListName == listName);
                CommentedLinks = documents.OrderByDescending(d => d.Date);
            }
            else
            {
                IEnumerable<CommentedLinkItem> documents = await repository.GetDocuments(d => d.ListName == listName && d.Category == category);
                CommentedLinks = documents.OrderByDescending(d => d.Date);
            }

        }
        // only on page level [Authorize(KnownRoles.Admin)]
        public async Task<IActionResult> OnGetDeleteLinkAsync(string documentid)
        {
            await repository.DeleteDocumentAsync(documentid);
            ViewData["Message"] = "Eintrag gelöscht";

            return RedirectToPage();
        }
    }
}