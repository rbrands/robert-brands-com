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

namespace robert_brands_com.Pages.Termine
{
    [Authorize(Policy = KnownRoles.PolicyIsCalendarCoordinator)]
    public class NewEventModel : PageModel
    {
        const string EventCategoriesName = "TermineAuswahl";
        private ICosmosDBRepository<CalendarItem> _repository;
        private ICosmosDBRepository<ListCategory> _categoryRepository;
        private IActivityLog _activityLog;
        public IEnumerable<ListCategory> Categories { get; private set; }

        [BindProperty]
        public CalendarItem NewEvent { get; set; }

        public NewEventModel(ICosmosDBRepository<CalendarItem> calendarRepository, ICosmosDBRepository<ListCategory> categoryRepository, IActivityLog activityLogRepository)
        {
            _repository = calendarRepository;
            _categoryRepository = categoryRepository;
            _activityLog = activityLogRepository;
            this.NewEvent = new CalendarItem();
            this.NewEvent.StartDate = DateTime.Today.AddDays(1.0).AddHours(12.0);
            this.NewEvent.EndDate = this.NewEvent.StartDate.AddHours(2.0);
            NewEvent.CalendarName = "Termin";
            NewEvent.TimeToLive = 16070400;
        }
        public async Task OnGetAsync()
        {
            Categories = await _categoryRepository.GetDocuments(d => d.ListName == EventCategoriesName);
            this.NewEvent.Host = this.User.Identity.Name;
        }
        public async Task<IActionResult> OnGetChangeAsync(string documentid)
        {
            if (String.IsNullOrEmpty(documentid))
            {
                return new NotFoundResult();
            }
            NewEvent = await _repository.GetDocument(documentid);
            if (null == NewEvent)
            {
                return new NotFoundResult();
            }
            Categories = await _categoryRepository.GetDocuments(d => d.ListName == EventCategoriesName);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(this.NewEvent.Host))
                {
                    this.NewEvent.Host = this.User.Identity.Name;
                }
                if (!String.IsNullOrEmpty(NewEvent.Id))
                {
                    CalendarItem existingCalendarItem = await _repository.GetDocument(NewEvent.Id);
                    if (null != existingCalendarItem)
                    {
                        NewEvent.Infos = existingCalendarItem.Infos;
                        NewEvent.Members = existingCalendarItem.Members;
                        NewEvent.RegistrationKeys = existingCalendarItem.RegistrationKeys;
                    }
                }
                await _repository.UpsertDocument(NewEvent);
                return RedirectToPage("Index");
            }
            else
            {
                Categories = await _categoryRepository.GetDocuments(d => d.ListName == EventCategoriesName);
                ViewData["Message"] = "Da stimmt was nicht.";
                return Page();
            }
        }

    }
}