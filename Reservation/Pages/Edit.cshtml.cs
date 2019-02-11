using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Reservation.Pages
{
    public class EditModel : PageModel
    {
        static HttpClient client = new HttpClient();

        public EditModel()
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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HttpResponseMessage response = await client.GetAsync($"api/reservation/{id}");
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
            HttpResponseMessage response = await client.GetAsync($"api/reservation/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}