using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using robert_brands_com.Models;
using robert_brands_com.Repositories;

namespace robert_brands_com.Pages.Blog
{
    [AllowAnonymous]
    public class ArtikelModel : LoggedPageModel
    {
        const string Blog = "Blog";
        private ICosmosDBRepository<Article> repository;
        private IFunctionSiteTools _functionSiteTools;
        public Article ReferencedArticle { get; private set; }
        public string Language { get; private set; }

        public ArtikelModel(ICosmosDBRepository<Article> articleRepository, IActivityLog activityLog, IFunctionSiteTools functionSiteTools) : base(activityLog, "Blog")
        {
            repository = articleRepository;
            _functionSiteTools = functionSiteTools;
        }
        public async Task<IActionResult> OnGetAsync(string permalink = null, string language = null)
        {
            if (String.IsNullOrEmpty(permalink))
            {
                return new NotFoundResult();
            }
            ReferencedArticle = (await repository.GetDocuments(d => d.ListName == Blog && d.UrlTitle == permalink.ToLower())).OrderByDescending(d => d.Date).FirstOrDefault();
            if (ReferencedArticle == null)
            {
                return new NotFoundResult();
            }
            Language = "de";
            if (!String.IsNullOrEmpty(language))
            {
                Language = language;
                ReferencedArticle.Title = await _functionSiteTools.Translate(language, ReferencedArticle.Title);
                ReferencedArticle.Summary = await _functionSiteTools.Translate(language, ReferencedArticle.Summary);
                if (!String.IsNullOrEmpty(ReferencedArticle.ArticleContent))
                {
                    ReferencedArticle.ArticleContent = await _functionSiteTools.Translate(language, ReferencedArticle.ArticleContent);
                }
            }
            this.ViewData["Keywords"] = this.ReferencedArticle.Tags;
            this.ViewData["Description"] = ReferencedArticle.Title;
            await this.LogActivity(permalink);
            return Page();
        }
        public async Task<IActionResult> OnGetShowArticleAsync(string documentid, string language = null)
        {
            if (String.IsNullOrEmpty(documentid))
            {
                return new NotFoundResult();
            }
            ReferencedArticle = await repository.GetDocument(documentid);
            if (ReferencedArticle == null)
            {
                return new NotFoundResult();
            }
            Language = "de";
            this.ViewData["Keywords"] = this.ReferencedArticle.Tags;
            this.ViewData["Description"] = ReferencedArticle.Title;
            if (!String.IsNullOrEmpty(language))
            {
                Language = language;
                ReferencedArticle.Title = await _functionSiteTools.Translate(language, ReferencedArticle.Title);
                ReferencedArticle.Summary = await _functionSiteTools.Translate(language, ReferencedArticle.Summary);
                if (!String.IsNullOrEmpty(ReferencedArticle.ArticleContent))
                {
                    ReferencedArticle.ArticleContent = await _functionSiteTools.Translate(language, ReferencedArticle.ArticleContent);
                }
            }
            await this.LogActivity(ReferencedArticle.UrlTitle ?? ReferencedArticle.Title);
            return Page();
        }
    }
}