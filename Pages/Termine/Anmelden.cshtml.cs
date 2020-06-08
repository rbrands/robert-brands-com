using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using reCAPTCHA.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using robert_brands_com.Models;
using robert_brands_com.Repositories;


namespace robert_brands_com.Pages.Termine
{
    [AllowAnonymous]
    public class AnmeldenModel : PageModel
    {
        private IRecaptchaService _recaptcha;
        private ICosmosDBRepository<CalendarItem> _repository;
        [BindProperty]
        public string CalendarItemId { get; set; }
        [BindProperty]
        public Member NewMember { get; set; }
        public CalendarItem ReferencedCalendarItem { get; set; }
        public bool UseCaptcha { get; private set; } = true;

        public AnmeldenModel(ICosmosDBRepository<CalendarItem> calendarRepository, IRecaptchaService recaptcha)
        {
            _repository = calendarRepository;
            _recaptcha = recaptcha;
        }
        public async Task<IActionResult> OnGetCheckInOutAsync(string documentid)
        {
            if (String.IsNullOrEmpty(documentid))
            {
                return new NotFoundResult();
            }
            CalendarItemId = documentid;
            ReferencedCalendarItem = await _repository.GetDocument(documentid);
            UseCaptcha = !User.Identity.IsAuthenticated && ReferencedCalendarItem.PublicListing;
            if (null == ReferencedCalendarItem || !ReferencedCalendarItem.RegistrationOpen && !User.IsInAnyRole(KnownRoles.CalendarCoordinatorRoles))
            {
                return new NotFoundResult();
            }
            List<Member> members = (ReferencedCalendarItem.Members != null) ? new List<Member>(ReferencedCalendarItem.Members) : new List<Member>();
            NewMember = new Member { UniqueId = Guid.NewGuid().ToString(), RegistrationDate = DateTime.UtcNow, Count = 1 };

            return Page();
        }
        public async Task<IActionResult> OnGetAsync(string permalink = null, string registrationkey = null)
        {
            if (String.IsNullOrEmpty(permalink))
            {
                return new NotFoundResult();
            }
            string permaLinkLower = permalink.ToLower();
            ReferencedCalendarItem = (await _repository.GetDocuments(d => d.UrlTitle == permaLinkLower)).OrderByDescending(d => d.StartDate).FirstOrDefault();
            if (null == ReferencedCalendarItem || !ReferencedCalendarItem.RegistrationOpen && !User.IsInAnyRole(KnownRoles.CalendarCoordinatorRoles))
            {
                return new NotFoundResult();
            }
            CalendarItemId = ReferencedCalendarItem.Id;
            UseCaptcha = !User.Identity.IsAuthenticated && ReferencedCalendarItem.PublicListing;
            List<Member> members = (ReferencedCalendarItem.Members != null) ? new List<Member>(ReferencedCalendarItem.Members) : new List<Member>();
            NewMember = new Member { UniqueId = Guid.NewGuid().ToString(), RegistrationDate = DateTime.UtcNow, Count = 1, RegistrationKey = registrationkey };

            return Page();
        }
        // only on page level [Authorize(Policy = KnownRoles.PolicyIsCalendarCoordinator)]
        public async Task<IActionResult> OnGetChangeAsync(string documentid, string registrationid)
        {
            if (String.IsNullOrEmpty(documentid) || String.IsNullOrEmpty(registrationid))
            {
                return new NotFoundResult();
            }
            CalendarItemId = documentid;
            ReferencedCalendarItem = await _repository.GetDocument(documentid);
            if (null == ReferencedCalendarItem)
            {
                return new NotFoundResult();
            }
            UseCaptcha = !User.Identity.IsAuthenticated && !ReferencedCalendarItem.PublicListing;
            List<Member> members = (ReferencedCalendarItem.Members != null) ? new List<Member>(ReferencedCalendarItem.Members) : new List<Member>();
            if (null == ReferencedCalendarItem.Members)
            {
                return new NotFoundResult();
            }
            NewMember = members.Find(c => c.UniqueId == registrationid);
            if (null == NewMember)
            {
                return new NotFoundResult();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostUnregisterAsync()
        {
            ReferencedCalendarItem = await _repository.GetDocument(CalendarItemId);
            UseCaptcha = !User.Identity.IsAuthenticated && null != ReferencedCalendarItem && ReferencedCalendarItem.PublicListing;
            if (UseCaptcha)
            {
                RecaptchaResponse captchaValid = await _recaptcha.Validate(Request);
                if (!captchaValid.success)
                {
                    ModelState.AddModelError("reCaptcha", "Bitte das reCaptcha bestätigen.");
                }
            }
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Schiefgegangen";
                return Page();
            }
            Member memberToUnregister;
            List<Member> members;
            if (null == ReferencedCalendarItem)
            {
                return new NotFoundResult();
            }
            members = (ReferencedCalendarItem.Members != null) ? new List<Member>(ReferencedCalendarItem.Members) : new List<Member>();
            memberToUnregister = members.Find(m => m.EMail == NewMember.EMail && m.Name == NewMember.Name);
            if (null == memberToUnregister)
            {
                ModelState.AddModelError("EMail", "E-Mail und Name stimmen mit keiner Anmeldung überein.");
                return Page();
            }
            if (ReferencedCalendarItem.RegistrationKeyRequired)
            {
                RegistrationKey checkKey = ReferencedCalendarItem.RegistrationKeys.FirstOrDefault(r => r.Key == NewMember.RegistrationKey);
                if (null == checkKey)
                {
                    ModelState.AddModelError("RegistrationKey", "Der angegebene Registrierungsschlüssel ist nicht zulässig.");
                    return Page();
                }
            }
            if (ModelState.IsValid)
            {
                members.RemoveAll(c => c.UniqueId == memberToUnregister.UniqueId);
                ReferencedCalendarItem.Members = members.OrderBy(m => m.RegistrationDate).ToArray();
                await _repository.UpsertDocument(ReferencedCalendarItem);
                ViewData["Message"] = "Abgemeldet";
                return RedirectToPage("Index", new { permalink = ReferencedCalendarItem.UrlTitle, message = "Anmeldung gelöscht." });
            }
            else
            {
                ViewData["Message"] = "Schiefgegangen";
                return Page();
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            ReferencedCalendarItem = await _repository.GetDocument(CalendarItemId);
            UseCaptcha = !User.Identity.IsAuthenticated && null != ReferencedCalendarItem && ReferencedCalendarItem.PublicListing;
            if (UseCaptcha)
            {
                RecaptchaResponse captchaValid = await _recaptcha.Validate(Request, false);
                if (!captchaValid.success)
                {
                    ModelState.AddModelError("reCaptcha", "Bitte das reCaptcha bestätigen.");
                }
            }
            if (ModelState.IsValid)
            {
                if (null == ReferencedCalendarItem)
                {
                    return new NotFoundResult();
                }
                if (ReferencedCalendarItem.RegistrationKeyRequired && !User.IsInAnyRole(KnownRoles.CalendarCoordinatorRoles))
                {
                    RegistrationKey checkKey = ReferencedCalendarItem.RegistrationKeys.FirstOrDefault(r => r.Key == NewMember.RegistrationKey);
                    if (null == checkKey)
                    {
                        ModelState.AddModelError("RegistrationKey", "Der angegebene Registrierungsschlüssel ist nicht zulässig.");
                        return Page();
                    }
                }
                List<Member> members = (ReferencedCalendarItem.Members != null) ? new List<Member>(ReferencedCalendarItem.Members) : new List<Member>();
                members.RemoveAll(c => c.UniqueId == NewMember.UniqueId);
                members.Add(NewMember);
                ReferencedCalendarItem.Members = members.OrderBy(m => m.RegistrationDate).ToArray();
                if (ReferencedCalendarItem.GetRegisteredMembersCount() > ReferencedCalendarItem.MaxRegistrationsCount && !User.IsInAnyRole(KnownRoles.CalendarCoordinatorRoles))
                {
                    ModelState.AddModelError("RegistrationCount", "Die Anzahl Anmeldungen überschreitet die maximale Teilnehmeranzahl.");
                    return Page();
                }
                await _repository.UpsertDocument(ReferencedCalendarItem);
                return RedirectToPage("Index", new { permalink = ReferencedCalendarItem.UrlTitle, message = "Anmeldung erfolgt." });
            }
            else
            {
                return Page();
            }
        }
    }
}