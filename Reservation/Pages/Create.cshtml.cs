using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Reservation.Pages
{
    public class CreateModel : PageModel
    {
        static HttpClient client = new HttpClient();

        public CreateModel()
        {
            if (client.BaseAddress is null)
            {
                client.BaseAddress = new Uri("https://localhost:44304/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            }
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

            HttpResponseMessage response = await client.GetAsync($"api/reservation/{id}");
            if (response.IsSuccessStatusCode)
            {
                Reservations = await response.Content.ReadAsAsync<IList<ReservationObj>>();
            }
            return Reservations.Any(e => e.ReservationId == id);
        }
    }
}