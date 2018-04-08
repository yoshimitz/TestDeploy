using LmycWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lmyc.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public int BoatId { get; set; }
        public Boat Boat { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
