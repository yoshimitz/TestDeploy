using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors;
using System.Security.Claims;
using System.Dynamic;
using LmycWeb.Data;
using Lmyc.Models;

namespace LymcWeb.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/Reservations")]
    [EnableCors("AllowAllOrigins")]
    public class ReservationsApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ReservationsApi
        [HttpGet]
        public IEnumerable<Object> GetBooking()
        {
            return _context.Bookings.ToList();
            /*List<Object> reservation = new List<Object>();
            foreach (Booking r in _context.Bookings)
            {
                dynamic b = new ExpandoObject();
                b.User = _context.Users.Find(r.User).UserName;
                b.Boat = _context.Boats.Find(r.Boat).BoatName; //r.Boat.BoatName
                b.BoatId = r.BoatId;
                b.BookingId = r.BookingId;
                b.StartDateTime = r.StartDateTime;
                b.EndDateTime = r.EndDateTime;

                reservation.Add(b);
            }
            return reservation;*/
        }

        // GET: api/ReservationsApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reservation = await _context.Bookings.SingleOrDefaultAsync(m => m.BookingId == id);

            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        // PUT: api/ReservationsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking([FromRoute] int id, [FromBody] Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != booking.BookingId)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ReservationsApi
        [HttpPost]
        public async Task<IActionResult> PostReservation([FromBody] Booking booking)
        {
            /*
             * CREATE METHOD
             * 
            booking.User = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Booking.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReservation", new { id = reservation.Id }, reservation);*/
            return null;
        }

        // DELETE: api/ReservationsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation([FromRoute] int id)
        {
            /*
             * CREATE METHOD
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reservation = await _context.Reservation.SingleOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservation.Remove(reservation);
            await _context.SaveChangesAsync();

            return Ok(reservation);*/
            return null;
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}