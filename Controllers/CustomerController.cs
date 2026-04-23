using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WebApplication2.Data;
using WebApplication2.Filters;
using WebApplication2.Models;
using Rotativa;
using System.Data.SqlClient;

namespace WebApplication2.Controllers
{
    [AuthorizeRole("User")]
    public class CustomerController : BaseController
    {
        private CarRentalDbContext db = new CarRentalDbContext();

        public ActionResult Dashboard()
        {

            return View();

        }
        public ActionResult AvailableCars(string sortOrder = "", int? minRating = null)
        {
            var availableCars = db.Cars.Where(c => c.IsAvailable).Include(c => c.Reviews).ToList();

            var carWithRatings = availableCars.Select(c => new
            {
                Car = c,
                AvgRating = c.Reviews.Any() ? c.Reviews.Average(r => r.Rating) : 0
            });

            if (minRating.HasValue)
            {
                carWithRatings = carWithRatings.Where(c => c.AvgRating >= minRating.Value);
            }

            if (!string.IsNullOrEmpty(sortOrder) && sortOrder == "rating_desc")
            {
                carWithRatings = carWithRatings.OrderByDescending(c => c.AvgRating);
            }
            var userId = Session["UserId"]?.ToString(); 
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }

        

            ViewBag.SortOrder = sortOrder;
            ViewBag.MinRating = minRating;

            return View(carWithRatings.Select(c => c.Car).ToList());
        }

        public ActionResult BookCar(int id)
        {
            var car = db.Cars.Find(id);
            if (car == null || !car.IsAvailable)
                return HttpNotFound();

            var booking = new Booking
            {
                CarId = car.Id,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1)
            };

            ViewBag.Car = car;
            return View(booking);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitBooking(Booking booking)
        {
            if (ModelState.IsValid)
            {
                // Get the car info
                var car = db.Cars.Find(booking.CarId);
                if (car == null)
                {
                    ModelState.AddModelError("", "Selected car not found.");
                    return View(booking);
                }

                // Set price per day using the car info
                decimal pricePerDay = car.PricePerDay;
                int rentalDays = (booking.EndDate - booking.StartDate).Days;
                decimal originalPrice = rentalDays * pricePerDay;

                // Set calculated values
                booking.TotalPrice = originalPrice;
                booking.DiscountedPrice = originalPrice;

                // Apply coupon if present
                if (!string.IsNullOrEmpty(booking.CouponCode))
                {
                    var coupon = db.Coupons
                        .FirstOrDefault(c => c.Code == booking.CouponCode && c.IsActive && c.ExpiryDate > DateTime.Now);

                    if (coupon != null)
                    {
                        var discountAmount = (originalPrice * coupon.DiscountPercentage) / 100;
                        booking.DiscountedPrice = originalPrice - discountAmount;
                    }
                }

                // Assign logged-in user ID from session (since no ASP.NET Identity)
                if (Session["UserId"] != null)
                {
                    booking.UserId = Convert.ToInt32(Session["UserId"]);
                }
                else
                {
                    ModelState.AddModelError("", "User is not logged in.");
                    return View(booking);
                }

                booking.Status = "Pending";
                booking.PaymentStatus = "Pending";

                db.Bookings.Add(booking);
                db.SaveChanges();

                return RedirectToAction("MyBookings");
            }

            return View(booking);
        }



        public ActionResult MyBookings()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "User"); 
            }

            int userId = (int)Session["UserId"];
            var bookings = db.Bookings
                             .Where(b => b.UserId == userId)
                             .Include("Car")
                             .ToList();
            var upcoming = bookings.FirstOrDefault(b => b.StartDate.Date == DateTime.Now.Date.AddDays(1));

            if (upcoming != null)
            {
                TempData["Reminder"] = $"Reminder: Your booking for {upcoming.Car.Make} {upcoming.Car.Model} starts tomorrow!";
            }

            return View(bookings);
        }
        public ActionResult Invoice(int id)
        {
            int userId = (int)Session["UserId"];
            var booking = db.Bookings.Include("Car").Include("User").FirstOrDefault(b => b.Id == id && b.UserId == userId);

            if (booking == null)
            {
                return HttpNotFound();
            }

            if (booking.Status != "Accepted")
            {
                return new HttpStatusCodeResult(403, "Invoice is only available for accepted bookings.");
            }

            return View(booking);
        }
        public ActionResult DownloadInvoice(int id)
        {
            int userId = (int)Session["UserId"];
            var booking = db.Bookings.Include("Car").Include("User")
                                      .FirstOrDefault(b => b.Id == id && b.UserId == userId);

            if (booking == null || booking.Status != "Accepted")
            {
                return new HttpStatusCodeResult(403);
            }

            return new ViewAsPdf("Invoice", booking)
            {
                FileName = $"Invoice_Booking_{booking.Id}.pdf"
            };
        }
        [HttpGet]
        public ActionResult ReportDamage(int bookingId)
        {
            var booking = db.Bookings.Include("Car").FirstOrDefault(b => b.Id == bookingId);
            if (booking == null)
                return HttpNotFound();

            var model = new DamageReport { BookingId = bookingId };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReportDamage(DamageReport model, int bookingId)
        {
            if (ModelState.IsValid)
            {
                model.BookingId = bookingId;
                model.ReportDate = DateTime.Now;
                model.Status = "Pending";
                db.DamageReports.Add(model);
                db.SaveChanges();

                TempData["Message"] = "Damage report submitted successfully.";
                return RedirectToAction("MyBookings","Customer");
            }

            return View(model);
        }

        public ActionResult MyClaims()
        {
            var user = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            var claims = db.DamageReports
                .Include("Booking")
                .Include("Booking.Car")
                .Where(r => r.Booking.UserId == user.Id)
                .ToList();
            return View(claims);
        }


    }
}
