using System.ComponentModel.DataAnnotations;

namespace Reservation
{
    public class ReservationObj
    {
        public int ReservationId { get; set; }
        public string ClientName { get; set; }
        [Required]
        public string Location { get; set; }

    }
}
