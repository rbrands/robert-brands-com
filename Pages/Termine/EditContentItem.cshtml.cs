using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using robert_brands_com.Models;
using robert_brands_com.Repositories;

namespace robert_brands_com.Pages.Termine
{
    [Authorize(Policy = KnownRoles.PolicyIsCalendarCoordinator)]
    public class EditContentItemModel : PageModel
    {
        private ICosmosDBRepository<CalendarItem> _repository;
        [BindProperty]
        public string CalendarItemId { get; set; }
        [BindProperty]
        public ContentItem AppointmentDetail { get; set; }

        public EditContentItemModel(ICosmosDBRepository<CalendarItem> calendarRepository)
        {
            _repository = calendarRepository;
        }
        public async Task<IActionResult> OnGetAsync(string documentid, string contentitemid)
        {
            if (String.IsNullOrEmpty(documentid))
            {
                return new NotFoundResult();
            }
            CalendarItemId = documentid;
            CalendarItem calendarItem = await _repository.GetDocument(documentid);
            if (null == calendarItem)
            {
                return new NotFoundResult();
            }
            List<ContentItem> contentItems = (calendarItem.Infos != null) ? new List<ContentItem>(calendarItem.Infos) : new List<ContentItem>();
            if (String.IsNullOrEmpty(contentitemid))
            {
                AppointmentDetail = new ContentItem { ContentType = ContentType.Text, UniqueId = Guid.NewGuid().ToString(), SortOrder = 10 };
                if (contentItems.Count > 0)
                {
                    AppointmentDetail.SortOrder = contentItems.Last().SortOrder + 10;
                }
            }
            else
            {
                if (null == calendarItem.Infos)
                {
                    return new NotFoundResult();
                }
                AppointmentDetail = contentItems.Find(c => c.UniqueId == contentitemid);
                if (null == AppointmentDetail)
                {
                    return new NotFoundResult();
                }
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                CalendarItem calendarItem = await _repository.GetDocument(CalendarItemId);
                if (null == calendarItem)
                {
                    return new NotFoundResult();
                }
                List<ContentItem> contentItems = (calendarItem.Infos != null) ? new List<ContentItem>(calendarItem.Infos) : new List<ContentItem>();
                contentItems.RemoveAll(c => c.UniqueId == AppointmentDetail.UniqueId);
                contentItems.Add(AppointmentDetail);
                calendarItem.Infos = contentItems.OrderBy(c => c.SortOrder).ToArray();
                await _repository.UpsertDocument(calendarItem);
                return RedirectToPage("Index", new { permalink = calendarItem.UrlTitle });
            }
            else
            {
                ViewData["Message"] = "Da stimmt was nicht.";
                return Page();
            }
        }
    }
}