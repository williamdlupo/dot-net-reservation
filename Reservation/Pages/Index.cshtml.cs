using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Reservation.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public IConfiguration Configuration { get; }

        public IList<ReservationObj> Reservations { get; private set; }

        [TempData]
        public string Message { get; set; }

        public IndexModel(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            Configuration = configuration;

            _clientFactory = clientFactory;
        }

        public async Task OnGetAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/reservation");
            var client = _clientFactory.CreateClient("webApi");
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Reservations = await response.Content.ReadAsAsync<IList<ReservationObj>>();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/reservation/{id}");
            var client = _clientFactory.CreateClient("webApi");
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Message = $"Reservation Removed";
            }

            return RedirectToPage();
        }

        public IActionResult OnGetTwitterLogin()
        {
            return new ChallengeResult("Twitter",
                new AuthenticationProperties { RedirectUri = "/" });
        }

        public async Task<IActionResult> OnGetLogoutAsync()
        {
            await HttpContext.SignOutAsync("Cookies",
                new AuthenticationProperties { RedirectUri = "/" });

            return RedirectToPage();
        }
    }
}
