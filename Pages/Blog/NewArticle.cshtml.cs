using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using robert_brands_com.Models;
using robert_brands_com.Repositories;


namespace robert_brands_com.Pages.Blog
{
    [Authorize(Policy = KnownRoles.PolicyIsBlogAuthor)]
    public class NewArticleModel : LoggedPageModel
    {
        private ICosmosDBRepository<Article> repository;
        private ICosmosDBRepository<ArticleTag> tagRepository;
        private IConfiguration configuration;
        public IEnumerable<Article> Tracks { get; private set; }
        [BindProperty]
        public string CallingPage { get; set; }
        [BindProperty]
        public string ListName { get; set; }
        [BindProperty]
        public bool FeaturedEnabled { get; set; }
        [BindProperty]
        public bool NicknameEnabled { get; set; }
        [BindProperty]
        public bool TitleLinkEnabled { get; set; }
        [BindProperty]
        public bool SummaryEnabled { get; set; }
        [BindProperty]
        public bool LinkEnabled { get; set; }
        [BindProperty]
        public bool TagsEnabled { get; set; }
        [BindProperty]
        public bool ImageEnabled { get; set; }
        [BindProperty]
        public Article NewArticle { get; set; }

        public NewArticleModel(ICosmosDBRepository<Article> repositoryService, ICosmosDBRepository<ArticleTag> tagRepository, IActivityLog activityLogRepository, IConfiguration configuration) : base(activityLogRepository, "NewArticle")
        {
            this.repository = repositoryService;
            this.tagRepository = tagRepository;
            this.NewArticle = new Article();
            this.configuration = configuration;
        }
        public async Task<IActionResult> OnGetChangeLinkAsync(string documentid)
        {
            if (String.IsNullOrEmpty(documentid))
            {
                return new NotFoundResult();
            }
            NewArticle = await repository.GetDocument(documentid);
            if (null == NewArticle)
            {
                return new NotFoundResult();
            }
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (String.IsNullOrEmpty(NewArticle.Summary) && String.IsNullOrEmpty(NewArticle.ArticleContent))
            {
                ModelState.AddModelError(nameof(NewArticle.ArticleContent), "Zusammenfassung und/oder Artikeltext eingeben.");
            }
            if (!String.IsNullOrEmpty(NewArticle.FlickrLink) && String.IsNullOrEmpty(NewArticle.FlickrLinkImage))
            {
                ModelState.AddModelError(nameof(NewArticle.FlickrLinkImage), "Wenn ein Flick-Link angegeben wird, muss auch ein Image-Link angegeben werden.");
            }
            if (ModelState.IsValid)
            {
                this.NewArticle.Author = this.User.Identity.Name;
                NewsConfig newsConfig = configuration.GetSection("NewsConfig:" + NewArticle.ListName).Get<NewsConfig>();
                if (!User.IsInAnyRole(newsConfig.WriteAccess.Split(',')))
                {
                    return new UnauthorizedResult();
                }
                await this.repository.UpsertDocument(NewArticle);
                if (!String.IsNullOrEmpty(this.NewArticle.Tags))
                {
                    UpdateTags(NewArticle.Tags, NewArticle.ListName);
                }
                await this.LogActivity($"Article in {NewArticle.ListName} posted.");

                return RedirectToPage(CallingPage);
            }
            else
            {
                ViewData["Message"] = "Da stimmt was nicht.";
                return Page();
            }
        }
        public async void UpdateTags(string tags, string listName)
        {
            string[] tagList = tags.Split(',');
            foreach (string tag in tagList)
            {
                string trimmedTag = tag.Trim();
                if (!String.IsNullOrWhiteSpace(trimmedTag))
                {
                    string logicalKey = listName + "-" + trimmedTag;
                    ArticleTag articleTag = await tagRepository.GetDocumentByKey(logicalKey);
                    if (null == articleTag)
                    {
                        articleTag = new ArticleTag();
                        articleTag.LogicalKey = logicalKey;
                        articleTag.TagCount = 1;
                        articleTag.Tag = trimmedTag;
                    }
                    else
                    {
                        articleTag.TagCount += 1;
                    }
                    articleTag.ListName = listName;
                    articleTag.TimeToLive = 3600 * 24 * 320; // Tags not used longer than one year can be deleted
                    await tagRepository.UpsertDocument(articleTag);
                }
            }
        }

        public async Task<IActionResult> OnGet(string callingPage, string listName, string logicalKey)
        {
            if (String.IsNullOrEmpty(listName) || String.IsNullOrEmpty(callingPage))
            {
                return new NotFoundResult();
            }
            NewsConfig newsConfig = configuration.GetSection("NewsConfig:" + listName).Get<NewsConfig>();
            if (!User.IsInAnyRole(newsConfig.WriteAccess.Split(',')))
            {
                return new UnauthorizedResult();
            }
            await this.PrepareArticle(callingPage, listName, logicalKey, null);
            return Page();
        }
        public async Task<JsonResult> OnGetTagsAsync(string listname)
        {
            IEnumerable<ArticleTag> tags = await this.tagRepository.GetDocuments(d => d.ListName == listname);
            List<string> tagsAsStringList = new List<string>();
            foreach (ArticleTag tag in tags)
            {
                tagsAsStringList.Add(tag.Tag);
            }
            return new JsonResult(tagsAsStringList);
        }
        private async Task PrepareArticle(string callingPage, string listName, string logicalKey, string documentId)
        {
            CallingPage = callingPage;
            ListName = listName;
            if (!String.IsNullOrEmpty(logicalKey))
            {
                NewArticle.LogicalKey = logicalKey;
                Article existentDocument = await repository.GetDocumentByKey(logicalKey);
                if (null != existentDocument)
                {
                    NewArticle = existentDocument;
                }
            }
            else if (!String.IsNullOrEmpty(documentId))
            {
                NewArticle = await repository.GetDocument(documentId);
            }
            NewArticle.ListName = listName;
        }
        public async Task<IActionResult> OnGetCustomized(string callingPage, string listName, string logicalKey, string documentId,
                                                         int? timeToLive,
                                                         bool? titleLink,
                                                         bool? summary,
                                                         bool? link,
                                                         bool? tags,
                                                         bool? image,
                                                         bool? featuredEnabled,
                                                         bool? nickname)
        {
            if (String.IsNullOrEmpty(listName) || String.IsNullOrEmpty(callingPage))
            {
                return new NotFoundResult();
            }
            NewsConfig newsConfig = configuration.GetSection("NewsConfig:" + listName).Get<NewsConfig>();
            if (!User.IsInAnyRole(newsConfig.WriteAccess.Split(',')))
            {
                return new UnauthorizedResult();
            }
            await this.PrepareArticle(callingPage, listName, logicalKey, documentId);
            if (null != timeToLive && 0 < timeToLive)
            {
                NewArticle.TimeToLive = timeToLive;
            }
            if (null != featuredEnabled && featuredEnabled == true)
            {
                FeaturedEnabled = true;
            }
            /* rbrands: nickname not used
            if (null != nickname && nickname == true)
            {
                NicknameEnabled = true;
            }
            */
            if (null != titleLink && titleLink == true)
            {
                TitleLinkEnabled = true;
            }
            if (null != summary && summary == true)
            {
                SummaryEnabled = true;
            }
            if (null != link && link == true)
            {
                LinkEnabled = true;
            }
            if (null != tags && tags == true)
            {
                TagsEnabled = true;
            }
            if (null != image && image == true)
            {
                ImageEnabled = true;
            }
            if (null != image && image == true)
            {
                ImageEnabled = true;
            }
            return Page();
        }
    }
}