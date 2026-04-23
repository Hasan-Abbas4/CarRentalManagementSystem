using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class ReviewController : BaseController
    {
            private readonly CarRentalDbContext db = new CarRentalDbContext();

            // GET: Review/Create
            [HttpGet]
            public ActionResult Create(int? carId, int? bookingId)
            {
                if (!carId.HasValue || carId.Value == 0)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid Car ID");

                var booking = db.Bookings.FirstOrDefault(b => b.Id == bookingId && b.PaymentStatus == "Paid");
                if (booking == null)
                {
                    TempData["Message"] = "You cannot leave a review for this car as your booking is either incomplete or doesn't exist.";
                    return RedirectToAction("MyBookings", "Customer");
                }

                var review = new Review
                {
                    CarId = carId.Value,
                };

                return View(review);
            }

            // POST: Review/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Create(Review review)
            {
            if (ModelState.IsValid)
                {
                review.UserId = (int)Session["UserId"];
                review.DatePosted = DateTime.Now;
                    db.Reviews.Add(review);
                    db.SaveChanges();
                    TempData["Message"] = "Thank you for your review!";
                    return RedirectToAction("MyBookings", "Customer", new { id = review.CarId });
                }

                // If there are validation errors, return to the view with the review data
                return View(review);
            }
        }

 }

