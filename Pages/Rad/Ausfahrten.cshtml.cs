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
            // Set defaults for meta tags
            ViewData["Title"] = "Ausfahrten";
            ViewData["Description"] = "Hier sammle ich alles rund um die Ausfahrten der letzten Zeit - Fotos, Video, GPS-Tracks alles auf einer Seite.";
            ViewData["Image"] = "https://live.staticflickr.com/65535/54172429130_e75fc2b5cf_h.jpg";

            if (String.IsNullOrEmpty(category))
            {
                IEnumerable<TrackItem> documents = await repository.GetDocuments(d => d.ListName == "Ausfahrten");
                Tracks = documents.OrderByDescending(d => d.Date);
                await this.LogGetActivity();
            }
            else if (String.IsNullOrEmpty(permalink))
            {
                string categoryLower = category.ToLower();
                IEnumerable<TrackItem> documents = await repository.GetDocuments(d => d.ListName == "Ausfahrten" && d.Category.ToLower() == categoryLower);
                Tracks = documents.OrderByDescending(d => d.Date);
                this.ViewData["Title"] = "Ausfahrten " + category;
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
                    // Don't support translation for now
                    // ReferencedTrack.Description = await _functionSiteTools.Translate(language, ReferencedTrack.Description);
                }
                this.ViewData["Title"] = ReferencedTrack.Title;
                this.ViewData["Description"] = "Die Tourbeschreibung.";
                if (!String.IsNullOrEmpty(ReferencedTrack.KomootTourImage))
                {
                    this.ViewData["Image"] = ReferencedTrack.KomootTourImage;
                }
                else if (!String.IsNullOrEmpty(ReferencedTrack.ImageLink))
                {
                    this.ViewData["Image"] = ReferencedTrack.ImageLink;
                }
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