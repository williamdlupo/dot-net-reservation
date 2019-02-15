using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Reservation.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        public IConfiguration Configuration { get; }

        public CreateModel(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            Configuration = configuration;
            _clientFactory = clientFactory;
        }

        [TempData]
        public string Message { get; set; }

        [BindProperty]
        public ReservationObj Reservation { get; set; }

        public IActionResult OnGetAsync(int? id)
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var client = _clientFactory.CreateClient("webApi");
                HttpResponseMessage response = await client.PostAsJsonAsync($"api/reservation", Reservation);

                if (response.IsSuccessStatusCode)
                {
                    Reservation = await response.Content.ReadAsAsync<ReservationObj>();
                }

                Message = $"Reservation for {Reservation.ClientName} in {Reservation.Location} has been added!";
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
            IList<ReservationObj> Reservations = new List<ReservationObj>();

            var request = new HttpRequestMessage(HttpMethod.Get, $"api/reservation/{id}");
            var client = _clientFactory.CreateClient("webApi");
            HttpResponseMessage response = await client.SendAsync(request);

            return response.IsSuccessStatusCode;

        }
    }
}