using LmycWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lmyc.Models
{
    public class Boat
    {
        public int BoatId { get; set; }

        [Required]
        [Display(Name = "Boat Name")]
        public string BoatName { get; set; }

        [Display(Name = "Picture")]
        public string Picture { get; set; }

        [Required]
        [Display(Name = "Length (Feet)")]
        public int LengthInFeet { get; set; }

        [Required]
        [Display(Name = "Make")]
        public string Make { get; set; }

        [Required]
        [Display(Name = "Year")]
        public int Year { get; set; }

        [Display(Name = "Creation Date")]
        public DateTime RecordCreationDate { get; set; }

        [ForeignKey("User")]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        public virtual ApplicationUser User { get; set; }

        [HiddenInput(DisplayValue = false)]
        public List<Booking> Bookings { get; set; }
    }
}
