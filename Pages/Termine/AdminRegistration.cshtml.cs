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
    public class AdminRegistrationModel : PageModel
    {
        private ICosmosDBRepository<CalendarItem> _calendarRepository;
        public CalendarItem ReferencedCalenderItem { get; private set; }
        public List<Member> RegisteredUsers { get; private set; } = new List<Member>();
        public List<RegistrationKey> RegistrationKeys { get; private set; } = new List<RegistrationKey>();
        [BindProperty]
        public RegistrationKey NewRegistrationKey { get; set; }
        [BindProperty]
        public string CalendarItemId { get; set; }
        public AdminRegistrationModel(ICosmosDBRepository<CalendarItem> calendarRepository)
        {
            _calendarRepository = calendarRepository;
        }

        public async Task<IActionResult> OnGetAsync(string documentid)
        {
            if (String.IsNullOrEmpty(documentid))
            {
                return new NotFoundResult();
            }
            ReferencedCalenderItem = await _calendarRepository.GetDocument(documentid);
            CalendarItemId = ReferencedCalenderItem.Id;
            if (ReferencedCalenderItem == null)
            {
                return new NotFoundResult();
            }
            if (ReferencedCalenderItem.Members != null)
            {
                RegisteredUsers.AddRange(ReferencedCalenderItem.Members);
            }
            if (ReferencedCalenderItem.RegistrationKeys != null)
            {
                RegistrationKeys.AddRange(ReferencedCalenderItem.RegistrationKeys);
            }
            Random randomGen = new Random();
            NewRegistrationKey = new RegistrationKey { UniqueId = Guid.NewGuid().ToString(), Key = randomGen.Next(1000, 1000000).ToString() };
            return Page();
        }
        public async Task<IActionResult> OnPostAddRegistrationKeyAsync()
        {
            ReferencedCalenderItem = await _calendarRepository.GetDocument(CalendarItemId);
            if (null == ReferencedCalenderItem)
            {
                return new NotFoundResult();
            }
            if (ModelState.IsValid)
            {
                List<RegistrationKey> members = (ReferencedCalenderItem.RegistrationKeys != null) ? new List<RegistrationKey>(ReferencedCalenderItem.RegistrationKeys) : new List<RegistrationKey>();
                members.RemoveAll(c => c.Key == NewRegistrationKey.Key);
                members.Add(NewRegistrationKey);
                ReferencedCalenderItem.RegistrationKeys = members.ToArray();
                await _calendarRepository.UpsertDocument(ReferencedCalenderItem);
                return RedirectToPage(new { documentid = CalendarItemId });
            }
            else
            {
                ViewData["Message"] = "Schiefgegangen";
                return Page();
            }
        }

        public async Task<IActionResult> OnGetDeleteAsync(string documentid, string memberid)
        {
            if (String.IsNullOrEmpty(documentid))
            {
                return new NotFoundResult();
            }
            ReferencedCalenderItem = await _calendarRepository.GetDocument(documentid);
            if (ReferencedCalenderItem == null || ReferencedCalenderItem.Members == null)
            {
                return new NotFoundResult();
            }
            RegisteredUsers.AddRange(ReferencedCalenderItem.Members);
            RegisteredUsers.RemoveAll(m => m.UniqueId == memberid);
            ReferencedCalenderItem.Members = RegisteredUsers.ToArray();
            await _calendarRepository.UpsertDocument(ReferencedCalenderItem);
            return RedirectToPage(new { documentid = ReferencedCalenderItem.Id });
        }
        public async Task<IActionResult> OnGetDeleteRegistrationKeyAsync(string documentid, string registrationkeyid)
        {
            if (String.IsNullOrEmpty(documentid))
            {
                return new NotFoundResult();
            }
            ReferencedCalenderItem = await _calendarRepository.GetDocument(documentid);
            if (ReferencedCalenderItem == null || ReferencedCalenderItem.RegistrationKeys == null)
            {
                return new NotFoundResult();
            }
            RegistrationKeys.AddRange(ReferencedCalenderItem.RegistrationKeys);
            RegistrationKeys.RemoveAll(m => m.UniqueId == registrationkeyid);
            ReferencedCalenderItem.RegistrationKeys = RegistrationKeys.ToArray();
            await _calendarRepository.UpsertDocument(ReferencedCalenderItem);
            return RedirectToPage(new { documentid = ReferencedCalenderItem.Id });
        }
    }
}