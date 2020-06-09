using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using robert_brands_com.Models;
using robert_brands_com.Repositories;

namespace robert_brands_com.Pages.Blog
{
    [AllowAnonymous]
    public class IndexModel : LoggedPageModel
    {
        const string Blog = "Blog";
        private ICosmosDBRepository<Article> repository;
        private ICosmosDBRepository<ArticleTag> tagsRepository;
        public Article HeadlineArticle { get; set; }
        public Article SideFeaturedArticle_1 { get; set; }
        public Article SideFeaturedArticle_2 { get; set; }
        public List<Article> BlogArticles { get; private set; }
        public List<string> Tags { get; private set; }
        public string Tag { get; private set; }
        public string ContinuationToken { get; set; }

        public IndexModel(ICosmosDBRepository<Article> repositoryService, ICosmosDBRepository<ArticleTag> tagService, IActivityLog activityLogRepository) : base(activityLogRepository, "Blog")
        {
            this.repository = repositoryService;
            this.tagsRepository = tagService;
        }

        public async Task OnGetAsync(string tag = null)
        {
            await this.LogGetActivity();
            // await ReadArticlesAsync(tag);
            await ReadArticlePagedAsync(null, tag);
            await ReadTags();
        }
        public async Task OnGetOlderAsync(string continuationToken, string tag)
        {
            await this.LogGetActivity();
            await ReadArticlePagedAsync(continuationToken, tag);
            await ReadTags();
        }

        private async Task ReadArticlesAsync(string tag)
        {
            BlogArticles = new List<Article>(10);
            IEnumerable<Article> documents;
            if (!String.IsNullOrEmpty(tag))
            {
                documents = (await repository.GetDocuments(d => d.ListName == Blog)).Where(d => d.Tags?.IndexOf(tag, StringComparison.OrdinalIgnoreCase) >= 0).OrderByDescending(d => d.Date).Take(50);
            }
            else
            {
                documents = (await repository.GetDocuments(d => d.ListName == Blog)).OrderByDescending(d => d.Date);
            }
            foreach (Article article in documents)
            {
                // Set featured articles
                if (null == HeadlineArticle && article.FeaturedLevel == 1)
                {
                    HeadlineArticle = article;
                }
                else if (null == SideFeaturedArticle_1 && article.FeaturedLevel == 2)
                {
                    SideFeaturedArticle_1 = article;
                }
                else if (null == SideFeaturedArticle_2 && article.FeaturedLevel == 3)
                {
                    SideFeaturedArticle_2 = article;
                }
                else
                {
                    BlogArticles.Add(article);
                }
            }
        }
        private async Task ReadArticlePagedAsync(string continuationToken, string tag)
        {
            BlogArticles = new List<Article>();
            PagedResult<Article> pagedResult;
            IEnumerable<Article> documents;
            if (!String.IsNullOrEmpty(tag))
            {
                string tagLowercase = tag.ToLower();
                pagedResult = await repository.GetPagedDocumentsDescending<DateTime>(d => d.ListName == Blog && d.Tags.ToLower().Contains(tagLowercase), d => d.Date, 25, continuationToken);
                documents = pagedResult.Result;
            }
            else
            {
                pagedResult = await repository.GetPagedDocumentsDescending<DateTime>(d => d.ListName == Blog, d => d.Date, 25, continuationToken);
                documents = pagedResult.Result;
            }
            ContinuationToken = pagedResult.ContinuationToken;
            Tag = tag;
            foreach (Article article in documents)
            {
                // Set featured articles
                if (null == HeadlineArticle && article.FeaturedLevel == 1)
                {
                    HeadlineArticle = article;
                }
                else if (null == SideFeaturedArticle_1 && article.FeaturedLevel == 2)
                {
                    SideFeaturedArticle_1 = article;
                }
                else if (null == SideFeaturedArticle_2 && article.FeaturedLevel == 3)
                {
                    SideFeaturedArticle_2 = article;
                }
                else
                {
                    BlogArticles.Add(article);
                }
            }
        }

        private async Task ReadTags()
        {
            IEnumerable<ArticleTag> tags = (await this.tagsRepository.GetDocuments(d => d.ListName == Blog)).OrderByDescending(d => d.TagCount).Take(20);
            Tags = new List<string>();
            foreach (ArticleTag tag in tags)
            {
                Tags.Add(tag.Tag);
            }
        }

        // only on page level [Authorize(Policy = KnownRoles.PolicyIsBlogAuthor)]
        public async Task<IActionResult> OnGetDeleteArticleAsync(string documentid)
        {
            if (String.IsNullOrEmpty(documentid))
            {
                return new NotFoundResult();
            }
            await repository.DeleteDocumentAsync(documentid);
            ViewData["Message"] = "Eintrag gelöscht";

            return RedirectToPage();
        }
    }
}