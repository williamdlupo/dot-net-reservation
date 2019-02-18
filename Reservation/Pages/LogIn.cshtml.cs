using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Reservation.Pages
{
    public class LogInModel : PageModel
    {

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
        

        public IActionResult OnGetAsync(string returnUrl = null)
        {
            return Page();
        }

        public IActionResult OnGetTwitterLogin()
        {
            return new ChallengeResult("Twitter",
                new AuthenticationProperties { RedirectUri = "/" });
        }
    }
}