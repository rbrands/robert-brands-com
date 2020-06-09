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


namespace robert_brands_com.Pages.Rad
{
    [Authorize(Roles = KnownRoles.Admin)]
    public class NewTrackLinkModel : PageModel
    {
        private ICosmosDBRepository<TrackItem> repository;
        private ICosmosDBRepository<ListCategory> categoryRepository;
        private IActivityLog activityLog;
        public IEnumerable<TrackItem> Tracks { get; private set; }
        public IEnumerable<ListCategory> Categories { get; private set; }

        [BindProperty]
        public TrackItem NewTrack { get; set; }


        public NewTrackLinkModel(ICosmosDBRepository<TrackItem> repositoryService, ICosmosDBRepository<ListCategory> categoryRepository, IActivityLog activityLogRepository)
        {
            this.repository = repositoryService;
            this.categoryRepository = categoryRepository;
            this.activityLog = activityLogRepository;
            this.NewTrack = new TrackItem();
            this.NewTrack.ListName = "Ausfahrten";
        }
        public async Task<IActionResult> OnGetChangeLinkAsync(string documentid)
        {
            if (String.IsNullOrEmpty(documentid))
            {
                return new NotFoundResult();
            }
            NewTrack = await repository.GetDocument(documentid);
            if (null == NewTrack)
            {
                return new NotFoundResult();
            }
            Categories = await categoryRepository.GetDocuments(d => d.ListName == "Ausfahrten");
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (String.IsNullOrEmpty(NewTrack.StravaLink) && String.IsNullOrEmpty(NewTrack.GpsiesLink) && String.IsNullOrEmpty(NewTrack.KomootLink))
            {
                ModelState.AddModelError("GPS Link", "Es muss mindestens einer der Links zu Strava, GPSies oder Komoot angegeben werden.");
            }
            if (String.IsNullOrEmpty(NewTrack.KomootLink) && !String.IsNullOrEmpty(NewTrack.KomootTourImage))
            {
                ModelState.AddModelError("Komoot Tour Bild", "Wenn das Komoot Tour Bild angegeben wird, muss auch die Komoot-Tour eingegeben werden.");
            }
            if (ModelState.IsValid)
            {
                this.NewTrack.Author = this.User.Identity.Name;
                await this.repository.UpsertDocument(NewTrack);
                string targetPage;
                switch (NewTrack.ListName)
                {
                    case "Ausfahrten":
                        targetPage = "/Rad/Ausfahrten";
                        break;
                    case "Reise":
                        targetPage = "/Rad/Reisen";
                        break;
                    case "Wanderungen":
                        targetPage = "/Wanderungen/Index";
                        break;
                    default:
                        targetPage = "./Index";
                        break;
                }
                return RedirectToPage(targetPage);
            }
            else
            {
                Categories = await categoryRepository.GetDocuments(d => d.ListName == "Ausfahrten");
                ViewData["Message"] = "Da stimmt was nicht.";
                return Page();
            }
        }


        public async Task OnGetAsync()
        {
            Categories = await categoryRepository.GetDocuments(d => d.ListName == "Ausfahrten");
        }
    }
}