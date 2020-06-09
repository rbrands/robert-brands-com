using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using reCAPTCHA.AspNetCore;

namespace robert_brands_com.Pages.Admin
{
    public class CaptchaModel : PageModel
    {
        private IRecaptchaService _recaptcha;

        public CaptchaModel(IRecaptchaService recaptcha)
        {
            _recaptcha = recaptcha;
        }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            RecaptchaResponse captchaValid = await _recaptcha.Validate(Request, false);
            if (!captchaValid.success)
            {
                ModelState.AddModelError("reCaptcha", "Bitte das reCaptcha bestätigen.");
            }
            if (ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                ViewData["Message"] = "Schiefgegangen";
                return Page();
            }
        }
    }
}