using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebApplication2.Data;
using WebApplication2.Filters;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [AuthorizeRole("Admin")]
    public class AdminController : BaseController
    {
        private CarRentalDbContext db = new CarRentalDbContext();
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult ManageUsers(User user)
        {
            var users = db.Users.ToList();
            return View(users);
        }
        public ActionResult DeleteUser(int id)
        {
            var user = db.Users.Find(id);
            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
            }
            return RedirectToAction("ManageUsers");
        }
        public ActionResult ViewBookings(string email = "", DateTime? startDate = null, int? driverId = null)
        {
            var bookings = db.Bookings
                .Include("User")
                .Include("Car")
                .Include("Driver")
                .AsQueryable();

            if (!string.IsNullOrEmpty(email))
                bookings = bookings.Where(b => b.User.Email.Contains(email));

            if (startDate.HasValue)
            {
                var start = startDate.Value;
                var end = start.AddDays(1);

                bookings = bookings.ToList()
                                    .Where(b => b.StartDate >= start && b.StartDate < end)
                                    .AsQueryable();
            }

            if (driverId.HasValue)
                bookings = bookings.Where(b => b.DriverId == driverId);

            ViewBag.DriverSelectList = new SelectList(db.Drivers.ToList(), "Id", "Name");
            ViewBag.Drivers = db.Drivers.ToList(); // for assigning driver in table

            ViewBag.SelectedEmail = email;
            ViewBag.SelectedDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.SelectedDriverId = driverId;

            return View(bookings.ToList());
        }


        [HttpPost]
        public ActionResult AssignDriver(int bookingId, int driverId)
        {
            var booking = db.Bookings.Find(bookingId);
            var driver = db.Drivers.Find(driverId);

            if (booking != null && driver != null && driver.IsAvailable)
            {
                booking.DriverId = driverId;
                driver.IsAvailable = false;

                db.Entry(booking).State = EntityState.Modified;
                db.Entry(driver).State = EntityState.Modified;
                db.SaveChanges();

                TempData["AdminAction"] = $"Driver {driver.Name} assigned to Booking #{booking.Id}.";
            }

            return RedirectToAction("ViewBookings");
        }


        public ActionResult DeleteBooking(int id)
        {
            var booking = db.Bookings.Find(id);
            if (booking != null)
            {
                db.Bookings.Remove(booking);
                db.SaveChanges();
            }
            return RedirectToAction("ViewBookings");
        }
        [HttpPost]
        public ActionResult UpdateBookingStatus(int id, string status)
        {
            var booking = db.Bookings.Include("Car").FirstOrDefault(b => b.Id == id);
            if (booking != null)
            {
                booking.Status = status;

                if (status == "Accepted" && booking.Car != null)
                {
                    booking.Car.IsAvailable = false;
                }
                else if ((status == "Rejected" || status == "Completed") && booking.Car != null)
                {
                    booking.Car.IsAvailable = true;

                    // Make the driver available again
                    if (booking.DriverId.HasValue)
                    {
                        var driver = db.Drivers.Find(booking.DriverId.Value);
                        if (driver != null)
                        {
                            driver.IsAvailable = true;
                            db.Entry(driver).State = EntityState.Modified;
                        }
                    }
                }

                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();

                TempData["AdminAction"] = $"Booking #{id} has been {status.ToLower()} successfully.";
            }

            return RedirectToAction("ViewBookings");
        }
        public ActionResult Coupons()
        {
            var coupons = db.Coupons.ToList();
            return View(coupons);
        }

        // GET: Admin/CreateCoupon
        public ActionResult CreateCoupon()
        {
            return View();
        }

        // POST: Admin/CreateCoupon
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCoupon(Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                db.Coupons.Add(coupon);
                db.SaveChanges();
                return RedirectToAction("Coupons");
            }
            return View(coupon);
        }

        // GET: Admin/EditCoupon/{id}
        public ActionResult EditCoupon(int id)
        {
            var coupon = db.Coupons.Find(id);
            if (coupon == null)
            {
                return HttpNotFound();
            }
            return View(coupon);
        }

        // POST: Admin/EditCoupon/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCoupon(Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(coupon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Coupons");
            }
            return View(coupon);
        }

        // GET: Admin/DeleteCoupon/{id}
        public ActionResult DeleteCoupon(int id)
        {
            var coupon = db.Coupons.Find(id);
            if (coupon == null)
            {
                return HttpNotFound();
            }
            return View(coupon);
        }

        // POST: Admin/DeleteCoupon/{id}
        [HttpPost, ActionName("DeleteCoupon")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCouponConfirmed(int id)
        {
            var coupon = db.Coupons.Find(id);
            if (coupon != null)
            {
                db.Coupons.Remove(coupon);
                db.SaveChanges();
            }
            return RedirectToAction("Coupons");
        }
        public ActionResult AllDamageReports()
        {
            var reports = db.DamageReports.Include("Booking").Include("Booking.Car").Include("Booking.User").ToList();
            return View(reports);
        }

        [HttpPost]
        public ActionResult ProcessClaim(int id, string action, decimal? repairCost)
        {
            var report = db.DamageReports.Find(id);
            if (report == null) return HttpNotFound();

            report.Status = action; // "Approved" or "Rejected"
            report.RepairCost = repairCost;
            report.CustomerCharged = action == "Approved" && repairCost.HasValue && repairCost.Value > 0;

            db.SaveChanges();
            return RedirectToAction("AllDamageReports");
        }

    }
}