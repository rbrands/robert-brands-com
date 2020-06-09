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
    public class SendMailModel : PageModel
    {
        private ICosmosDBRepository<CalendarItem> _calendarRepository;
        private IFunctionSiteTools _functionSiteTools;
        public CalendarItem ReferencedCalenderItem { get; private set; }
        [BindProperty]
        public string CalendarItemId { get; set; }
        [BindProperty]
        public string MailSubject { get; set; }
        [BindProperty]
        public string MailText { get; set; }

        public SendMailModel(ICosmosDBRepository<CalendarItem> calendarRepository, IFunctionSiteTools functionSiteTools)
        {
            _calendarRepository = calendarRepository;
            _functionSiteTools = functionSiteTools;
        }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return new NotFoundResult();
            }
            ReferencedCalenderItem = await _calendarRepository.GetDocument(id);
            CalendarItemId = ReferencedCalenderItem.Id;
            if (ReferencedCalenderItem == null)
            {
                return new NotFoundResult();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ReferencedCalenderItem = await _calendarRepository.GetDocument(CalendarItemId);
            if (null == ReferencedCalenderItem)
            {
                return new NotFoundResult();
            }
            if (ModelState.IsValid)
            {
                if (ReferencedCalenderItem.Members != null)
                {
                    foreach (Member member in ReferencedCalenderItem.Members)
                    {
                        await _functionSiteTools.SendMail(member.EMail, MailSubject, MailText);
                    }
                }
                return RedirectToPage("Index");
            }
            else
            {
                ViewData["Message"] = "Schiefgegangen";
                return Page();
            }
        }
    }
}