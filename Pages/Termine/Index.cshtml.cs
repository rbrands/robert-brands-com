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

namespace robert_brands_com.Pages.Termine
{
    [AllowAnonymous]
    public class IndexModel : LoggedPageModel
    {
        const string CategoryListName = "TermineAuswahl";
        private ICosmosDBRepository<CalendarItem> _calendarRepository;
        private ICosmosDBRepository<ListCategory> _categoryRepository;
        private ICosmosDBRepository<Article> _articleRepository;
        private IFunctionSiteTools _functionSiteTools;
        public IEnumerable<CalendarItem> CalendarItems { get; private set; }
        public CalendarItem ReferencedCalenderItem { get; private set; }
        public ContentItem HeaderItem { get; private set; }
        public IEnumerable<ListCategory> Categories { get; private set; }
        public Dictionary<string, string> CategoriesDisplay { get; private set; }
        public Article OverviewArticle { get; private set; }
        public string ArticleKey { get; private set; } = "termine";
        public string Message { get; set; }
        public IndexModel(ICosmosDBRepository<CalendarItem> calendarRepository, ICosmosDBRepository<ListCategory> categoryRepository, ICosmosDBRepository<Article> articleRepository, IActivityLog activityLog, IFunctionSiteTools functionSiteTools) : base(activityLog, "Termine")
        {
            _calendarRepository = calendarRepository;
            _categoryRepository = categoryRepository;
            _articleRepository = articleRepository;
            _functionSiteTools = functionSiteTools;

        }
        public async Task<IActionResult> OnGetAsync(string permalink = null, string language = null, string message = null)
        {
            Categories = await _categoryRepository.GetDocuments(d => d.ListName == CategoryListName);
            CategoriesDisplay = Categories.ToDictionary(t => t.Category, t => t.CategoryDisplay ?? t.Category);
            Message = message;
            IEnumerable<CalendarItem> documents;
            OverviewArticle = await _articleRepository.GetDocumentByKey(ArticleKey);
            DateTime compareDate = DateTime.Today.AddDays(-8);
            if (String.IsNullOrEmpty(permalink))
            {
                if (User.Identity.IsAuthenticated)
                {
                    documents = await _calendarRepository.GetDocuments();
                }
                else
                {
                    documents = await _calendarRepository.GetDocuments(d => d.EndDate > compareDate && d.PublicListing == true);
                }
                CalendarItems = documents.OrderBy(d => d.StartDate);
                await this.LogGetActivity();
            }
            else
            {
                string permaLinkLower = permalink.ToLower();
                ReferencedCalenderItem = (await _calendarRepository.GetDocuments(d => d.UrlTitle == permaLinkLower)).OrderByDescending(d => d.StartDate).FirstOrDefault();
                await this.LogActivity(permaLinkLower);
                if (ReferencedCalenderItem == null)
                {
                    // Now check if a category matches the given argument
                    documents = await _calendarRepository.GetDocuments(d => d.EndDate > compareDate && d.CalendarName.ToLower() == permaLinkLower && d.PublicListing == true);
                    if (null == documents)
                    {
                        return new NotFoundResult();
                    }
                    CalendarItems = documents.OrderBy(d => d.StartDate);
                    return Page();
                }
                ReferencedCalenderItem.Language = "de";
                if (!ReferencedCalenderItem.EnableTranslations)
                {
                    language = String.Empty;
                }
                if (!String.IsNullOrEmpty(language))
                {
                    ReferencedCalenderItem.Language = language;
                    if (language != "de")
                    {
                        ReferencedCalenderItem.Title = await _functionSiteTools.Translate(language, ReferencedCalenderItem.Title);
                        ReferencedCalenderItem.Summary = await _functionSiteTools.Translate(language, ReferencedCalenderItem.Summary);
                        ReferencedCalenderItem.Description = await _functionSiteTools.Translate(language, ReferencedCalenderItem.Description);
                    }
                }
                if (!ReferencedCalenderItem.PublicListing)
                {
                    ViewData["NoRobots"] = true;
                }
                if (ReferencedCalenderItem.Infos != null)
                {
                    List<ContentItem> infos = new List<ContentItem>(ReferencedCalenderItem.Infos);
                    this.HeaderItem = infos.Find(c => c.SortOrder == 0);
                    infos.ForEach(c => c.ReferenceId = ReferencedCalenderItem.Id);
                    if (ReferencedCalenderItem.Language != "de")
                    {
                        foreach (ContentItem c in infos)
                        {
                            c.Title = await _functionSiteTools.Translate(ReferencedCalenderItem.Language, c.Title);
                            c.Description = await _functionSiteTools.Translate(ReferencedCalenderItem.Language, c.Description);
                        }
                    }
                }

            }
            return Page();
        }
        // only on page level [Authorize(Policy = KnownRoles.PolicyIsCalendarCoordinator)]
        public async Task<IActionResult> OnGetDeleteAsync(string documentid)
        {
            await _calendarRepository.DeleteDocumentAsync(documentid);
            ViewData["Message"] = "Eintrag gelöscht";

            return RedirectToPage();
        }
        // only on page level [Authorize(Policy = KnownRoles.PolicyIsCalendarCoordinator)]
        public async Task<IActionResult> OnGetDeleteContentItemAsync(string documentid, string contentitemid)
        {
            CalendarItem calendarItem = await _calendarRepository.GetDocument(documentid);
            if (calendarItem == null)
            {
                return new NotFoundResult();
            }
            if (calendarItem.Infos != null)
            {
                List<ContentItem> infos = new List<ContentItem>(calendarItem.Infos);
                infos.RemoveAll(c => c.UniqueId == contentitemid);
                calendarItem.Infos = infos.ToArray();
                await _calendarRepository.UpsertDocument(calendarItem);
            }
            ViewData["Message"] = "Eintrag gelöscht";

            return RedirectToPage();
        }
    }
}
