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
    public class CollectionsModel : LoggedPageModel
    {
        private ICosmosDBRepository<TrackItem> repository;
        private ICosmosDBRepository<Article> articleRepository;
        public Article OverviewArticle { get; private set; }
        public IEnumerable<Article> CollectionsArticles { get; private set; }
        public string ArticleKey { get; private set; }
        public CollectionsModel(ICosmosDBRepository<TrackItem> trackRepository,ICosmosDBRepository<Article> articleRepository,  IActivityLog activityLog) : base(activityLog, "RadSammlung")
        {
            this.repository = trackRepository;
            this.articleRepository = articleRepository;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Set defaults for meta tags
            ViewData["Title"] = "Tour-Collections";
            ViewData["Description"] = "Sammlungen von Touren";
            ArticleKey = "tour-collections";
            OverviewArticle = await articleRepository.GetDocumentByKey(ArticleKey);
            this.ViewData["Title"] = OverviewArticle?.Title;
            if (!String.IsNullOrEmpty(OverviewArticle?.PlainSummary))
            {
                this.ViewData["Description"] = OverviewArticle?.PlainSummary;
            }
            if (!String.IsNullOrEmpty(OverviewArticle?.ImageLink))
            {
                this.ViewData["Image"] = OverviewArticle?.ImageLink;
            } 
            var allItems = await repository.GetDocuments(d => d.TourSet != null);
            var collections = allItems.AsQueryable().Select(d => d.TourSet).Distinct();
            CollectionsArticles = await articleRepository.GetDocuments(d => d.Id.StartsWith("Article-tourset-"));
            await this.LogGetActivity();
            return Page();
        }
    }
}
