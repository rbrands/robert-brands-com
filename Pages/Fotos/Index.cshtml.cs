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

namespace robert_brands_com.Pages.Fotos
{
    [AllowAnonymous]
    public class IndexModel : LoggedPageModel
    {
        private const string listName = "Fotos";
        private ICosmosDBRepository<CommentedLinkItem> repository;
        public IEnumerable<CommentedLinkItem> CommentedLinks { get; private set; }
        public CommentedLinkItem ReferencedPhotoPage { get; private set; }
        public ContentItem HeaderItem { get; private set; }

        public IndexModel(ICosmosDBRepository<CommentedLinkItem> repositoryService, IActivityLog activityLog) : base(activityLog, "Fotos")
        {
            this.repository = repositoryService;
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
                string permaLinkLower = category.ToLower();
                ReferencedPhotoPage = (await repository.GetDocuments(d => d.UrlTitle == permaLinkLower)).OrderByDescending(d => d.Date).FirstOrDefault();
                await this.LogActivity(permaLinkLower);
                if (ReferencedPhotoPage == null)
                {
                    // Now check if a category matches the given argument
                    IEnumerable<CommentedLinkItem> documents = await repository.GetDocuments(d => 
                        d.ListName.Equals(listName, StringComparison.OrdinalIgnoreCase) && 
                        d.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
                    CommentedLinks = documents.OrderByDescending(d => d.Date);
                }
                else
                {
                    if (ReferencedPhotoPage.Infos != null)
                    {
                        List<ContentItem> infos = new List<ContentItem>(ReferencedPhotoPage.Infos);
                        this.HeaderItem = infos.Find(c => c.SortOrder == 0);
                        infos.ForEach(c => c.ReferenceId = ReferencedPhotoPage.Id);
                    }
                }

            }
        }
        // only on Page level [Authorize(KnownRoles.Admin)]
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
        // only on page level [Authorize(KnownRoles.Admin)]
        public async Task<IActionResult> OnGetDeleteContentItemAsync(string documentid, string contentitemid)
        {
            CommentedLinkItem linktItem = await repository.GetDocument(documentid);
            if (linktItem == null)
            {
                return new NotFoundResult();
            }
            if (linktItem.Infos != null)
            {
                List<ContentItem> infos = new List<ContentItem>(linktItem.Infos);
                infos.RemoveAll(c => c.UniqueId == contentitemid);
                linktItem.Infos = infos.ToArray();
                await repository.UpsertDocument(linktItem);
            }
            ViewData["Message"] = "Eintrag gelöscht";

            return RedirectToPage();
        }

    }
}