using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Reservation.Pages
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        public IConfiguration Configuration { get; }

        public EditModel(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            Configuration = configuration;
            _clientFactory = clientFactory;
        }

        [TempData]
        public string Message { get; set; }

        [BindProperty]
        public ReservationObj Reservation { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/reservation/{id}");
            var client = _clientFactory.CreateClient("webApi");
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Reservation = await response.Content.ReadAsAsync<ReservationObj>();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var client = _clientFactory.CreateClient("webApi");
                HttpResponseMessage response = await client.PutAsJsonAsync($"api/reservation", Reservation);

                if (response.IsSuccessStatusCode)
                {
                    Message = $"Reservation for {Reservation.ClientName} has been updated to {Reservation.Location}";
                }
            }

            catch
            {
                if (await ReservationExists(Reservation.ReservationId))
                {
                    throw;
                }
                else
                {
                    return NotFound();
                }
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool> ReservationExists(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/reservation/{id}");
            var client = _clientFactory.CreateClient("webApi");
            HttpResponseMessage response = await client.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
    }
}