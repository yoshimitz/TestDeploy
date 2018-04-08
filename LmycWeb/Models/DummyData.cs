using LmycWeb.Data;
using LmycWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Lmyc.Models
{
    public class DummyData
    {
        public static List<Boat> GetBoats(string adminId1, string adminId2)
        {
            List<Boat> boats = new List<Boat>
            {
                new Boat
                {
                    BoatId = 1,
                    BoatName = "Sir Charles",
                    Picture = "https://en.wikipedia.org/wiki/Boat#/media/File:Lifeboat.17-31.underway.arp.jpg",
                    LengthInFeet = 30,
                    Make = "Amels",
                    Year = 2010,
                    RecordCreationDate = DateTime.Today,
                    CreatedBy = adminId1
                },


                new Boat
                {
                    BoatId = 2,
                    BoatName = "Lady of the Sea",
                    Picture = "https://upload.wikimedia.org/wikipedia/commons/1/14/Rodney_boat.jpg",
                    LengthInFeet = 35,
                    Make = "Feadship",
                    Year = 2000,
                    RecordCreationDate = DateTime.Today,
                    CreatedBy = adminId2
                },
                new Boat
                {
                    BoatId = 3,
                    BoatName = "Ace",
                    Picture = "https://www.pexels.com/photo/bay-boat-boats-dock-42092/",
                    LengthInFeet = 20,
                    Make = "Heesen",
                    Year = 2006,
                    RecordCreationDate = DateTime.Today,
                    CreatedBy = adminId2
                },
                new Boat
                {
                    BoatId = 4,
                    BoatName = "The Knight",
                    Picture = "https://www.pexels.com/photo/action-boat-jetski-leisure-625418/",
                    LengthInFeet = 30,
                    Make = "Christensen ",
                    Year = 1999,
                    RecordCreationDate = DateTime.Today,
                    CreatedBy = adminId1
                }
            };
            return boats;
        }

        public static List<Booking> GetBookings(ApplicationDbContext context)
        {
            List<Booking> bookings = new List<Booking>
            {
                new Booking
                {
                    BookingId = 1,
                    StartDateTime = new DateTime(2018, 6, 30, 6, 0, 0),
                    EndDateTime = new DateTime(2018, 6, 30, 12, 0, 0),
                    Boat = context.Boats.FirstOrDefault(b => b.BoatName == "Ace"),
                    User = context.Users.FirstOrDefault(u => u.UserName == "d")
                },
                new Booking
                {
                    BookingId = 2,
                    StartDateTime = new DateTime(2018, 7, 1, 20, 0, 0),
                    EndDateTime = new DateTime(2018, 7, 2, 6, 0, 0),
                    Boat = context.Boats.FirstOrDefault(b => b.BoatName == "The Knight"),
                    User = context.Users.FirstOrDefault(u => u.UserName == "m")
                },
                new Booking
                {
                    BookingId = 3,
                    StartDateTime = new DateTime(2018, 7, 30, 1, 0, 0),
                    EndDateTime = new DateTime(2018, 7, 30, 18, 0, 0),
                    Boat = context.Boats.FirstOrDefault(b => b.BoatName == "Ace"),
                    User = context.Users.FirstOrDefault(u => u.UserName == "m")
                }
            };
            return bookings;
        }
    }
}
