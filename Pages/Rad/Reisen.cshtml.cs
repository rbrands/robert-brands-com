using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using robert_brands_com.Repositories;
using robert_brands_com.Models;


namespace robert_brands_com.Pages.Rad
{
    [AllowAnonymous]
    public class ReisenModel : LoggedPageModel
    {
        const string ListName = "Reise";
        const string CategoryListName = "ReiseAuswahl";
        private ICosmosDBRepository<TrackItem> repository;
        private ICosmosDBRepository<ListCategory> categoryRepository;
        private ICosmosDBRepository<Article> articleRepository;
        private IFunctionSiteTools _functionSiteTools;
        public IEnumerable<TrackItem> Tracks { get; private set; }
        public TrackItem ReferencedTrack { get; private set; }
        public IEnumerable<ListCategory> Categories { get; private set; }
        public Dictionary<string, string> CategoriesDisplay { get; private set; }
        public Article OverviewArticle { get; private set; }
        public string ArticleKey { get; private set; }
        public ReisenModel(ICosmosDBRepository<TrackItem> trackRepository, ICosmosDBRepository<ListCategory> categoryRepository, ICosmosDBRepository<Article> articleRepository, IActivityLog activityLog, IFunctionSiteTools functionSiteTools) : base(activityLog, "RadReise")
        {
            this.repository = trackRepository;
            this.categoryRepository = categoryRepository;
            this.articleRepository = articleRepository;
            _functionSiteTools = functionSiteTools;
        }

        public async Task<IActionResult> OnGetAsync(string category = null, string permalink = null, string language = null)
        {
            IEnumerable<TrackItem> documents;
            if (String.IsNullOrEmpty(category))
            {
                documents = await repository.GetDocuments(d => d.ListName == ListName);
                ArticleKey = "radreisen";
                OverviewArticle = await articleRepository.GetDocumentByKey(ArticleKey);
                Tracks = documents.OrderByDescending(d => d.Date);
                this.ViewData["Title"] = "Rad - Reisen";
                this.ViewData["Description"] = "Unterwegs mit dem Rennrad - eine kleine Streckenauswahl";
                await this.LogGetActivity();
            }
            else if (String.IsNullOrEmpty(permalink))
            {
                string categoryLower = category.ToLower();
                documents = await repository.GetDocuments(d => d.ListName == ListName && d.Category.ToLower() == categoryLower);
                ArticleKey = "radreisen-" + categoryLower;
                OverviewArticle = await articleRepository.GetDocumentByKey(ArticleKey);
                if (null == OverviewArticle)
                {
                    OverviewArticle = await articleRepository.GetDocumentByKey("radreisen");
                }
                Tracks = documents.OrderByDescending(d => d.Date);
                this.ViewData["Title"] = $"Rad - Reisen - {category}";
                this.ViewData["Description"] = $"Rad - Reisen - {category}";
                await this.LogActivity(categoryLower);
            }
            else
            {
                string categoryLower = category.ToLower();
                string permaLinkLower = permalink.ToLower();
                ReferencedTrack = (await repository.GetDocuments(d => d.ListName == ListName && d.Category.ToLower() == categoryLower && d.UrlTitle == permaLinkLower)).OrderByDescending(d => d.Date).FirstOrDefault();
                if (ReferencedTrack == null)
                {
                    return new NotFoundResult();
                }
                ReferencedTrack.Language = "de";
                if (!String.IsNullOrEmpty(language))
                {
                    ReferencedTrack.Description = await _functionSiteTools.Translate(language, ReferencedTrack.Description);
                    ReferencedTrack.Language = language;
                }
                this.ViewData["Title"] = ReferencedTrack.Title;
                this.ViewData["Description"] = "Die Beschreibung zur Tour.";
                await this.LogActivity($"{categoryLower}/{permaLinkLower}");
            }
            Categories = await categoryRepository.GetDocuments(d => d.ListName == CategoryListName);
            CategoriesDisplay = Categories.ToDictionary(t => t.Category, t => t.CategoryDisplay ?? t.Category);
            if (null != Tracks)
            {
                foreach (TrackItem track in Tracks)
                {
                    if (CategoriesDisplay.ContainsKey(track.Category))
                    {
                        track.CategoryDisplay = CategoriesDisplay[track.Category];
                    }
                    else
                    {
                        track.CategoryDisplay = track.Category;
                    }
                }
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