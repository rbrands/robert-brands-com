using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using robert_brands_com.Repositories;
using robert_brands_com.Models;

namespace robert_brands_com.Pages.Rad
{
    [AllowAnonymous]
    public class AusfahrtenModel : LoggedPageModel
    {
        private ICosmosDBRepository<TrackItem> repository;
        private IFunctionSiteTools _functionSiteTools;
        public IEnumerable<TrackItem> Tracks { get; private set; }
        public TrackItem ReferencedTrack { get; private set; }
        public AusfahrtenModel(ICosmosDBRepository<TrackItem> repositoryService, IActivityLog activityLogRepository, IFunctionSiteTools functionSiteTools) : base(activityLogRepository, "Rad")
        {
            this.repository = repositoryService;
            _functionSiteTools = functionSiteTools;
        }
        public async Task<IActionResult> OnGetAsync(string category = null, string permalink = null, string language = null)
        {
            if (String.IsNullOrEmpty(category))
            {
                IEnumerable<TrackItem> documents = await repository.GetDocuments(d => d.ListName == "Ausfahrten");
                Tracks = documents.OrderByDescending(d => d.Date);
                this.ViewData["Title"] = "Radausfahrten";
                this.ViewData["Description"] = "Radausfahrten - Köln, Bergisches Land, Eifel";
                await this.LogGetActivity();
            }
            else if (String.IsNullOrEmpty(permalink))
            {
                string categoryLower = category.ToLower();
                IEnumerable<TrackItem> documents = await repository.GetDocuments(d => d.ListName == "Ausfahrten" && d.Category.ToLower() == categoryLower);
                Tracks = documents.OrderByDescending(d => d.Date);
                this.ViewData["Title"] = "Radausfahrten " + category;
                this.ViewData["Description"] = $"Radausfahrten - {category}";
                await this.LogActivity(categoryLower);
            }
            else
            {
                string categoryLower = category.ToLower();
                string permaLinkLower = permalink.ToLower();
                ReferencedTrack = (await repository.GetDocuments(d => d.ListName == "Ausfahrten" && d.Category.ToLower() == categoryLower && d.UrlTitle == permaLinkLower)).OrderByDescending(d => d.Date).FirstOrDefault();
                if (ReferencedTrack == null)
                {
                    return new NotFoundResult();
                }
                ReferencedTrack.Language = "de";
                if (!String.IsNullOrEmpty(language))
                {
                    ReferencedTrack.Language = language;
                    ReferencedTrack.Description = await _functionSiteTools.Translate(language, ReferencedTrack.Description);
                }
                this.ViewData["Title"] = ReferencedTrack.Title;
                this.ViewData["Description"] = "Die Tourbeschreibung.";
                if (!String.IsNullOrEmpty(ReferencedTrack.PlainDescription))
                {
                    this.ViewData["Description"] = ReferencedTrack.PlainDescription;
                }
                await this.LogActivity($"{categoryLower}/{permaLinkLower}");
            }
            return Page();
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