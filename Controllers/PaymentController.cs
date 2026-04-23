using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{

    public class PaymentController : BaseController
    {
        private CarRentalDbContext db = new CarRentalDbContext(); // your EF DbContext

        public ActionResult Pay(int bookingId)
        {
            var booking = db.Bookings.Find(bookingId);
            if (booking == null) return HttpNotFound();

            return View(booking); // Show mock payment form
        }

        [HttpPost]
        public ActionResult Pay(int bookingId, string decision)
        {
            var booking = db.Bookings.Find(bookingId);
            if (booking == null) return HttpNotFound();

            if (decision == "success")
            {
                booking.PaymentStatus = "Paid";
                db.SaveChanges();
                return RedirectToAction("Success");
            }
            else
            {
                booking.PaymentStatus = "Failed";
                db.SaveChanges();
                return RedirectToAction("Cancel");
            }
        }

        public ActionResult Success()
        {
            ViewBag.Message = "Payment successful.";
            return View();
        }

        public ActionResult Cancel()
        {
            ViewBag.Message = "Payment failed or cancelled.";
            return View();
        }

    }
}