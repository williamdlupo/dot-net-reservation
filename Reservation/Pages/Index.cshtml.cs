using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Reservation.Pages
{
    public class IndexModel : PageModel
    {
        static HttpClient client = new HttpClient();

        public IList<ReservationObj> Reservations { get; private set; }

        [TempData]
        public string Message { get; set; }

        public IndexModel()
        {
            if (client.BaseAddress is null)
            {
                client.BaseAddress = new Uri("https://localhost:44304/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        public async Task OnGetAsync()
        {
            HttpResponseMessage response = await client.GetAsync($"api/reservation");
            if (response.IsSuccessStatusCode)
            {
                Reservations = await response.Content.ReadAsAsync<IList<ReservationObj>>();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync($"api/reservation/{id}");
            if (response.IsSuccessStatusCode)
            {
                Message = $"Reservation Removed";
            }

            return RedirectToPage();
        }
    }
}
