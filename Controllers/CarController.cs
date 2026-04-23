using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Data;
using WebApplication2.Filters;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [AuthorizeRole("Admin")]
    public class CarController : BaseController
    {

        private CarRentalDbContext db = new CarRentalDbContext();

        public ActionResult Index()
        {
            var cars = db.Cars.ToList();
            return View(cars);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Car car)
        {
            if (ModelState.IsValid)
            {
                db.Cars.Add(car);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(car);
        }

        public ActionResult Edit(int id)
        {
            var car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Car car)
        {
            if (ModelState.IsValid)
            {
                db.Entry(car).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(car);
        }

        public ActionResult Delete(int id)
        {
            var car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            db.Cars.Remove(car);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Details(int id)
        {
            var car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }

            var reviews = db.Reviews.Where(r => r.CarId == id).ToList();
            ViewBag.Reviews = reviews;

            ViewBag.AvgRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;

            var user = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            if (user != null)
            {
                var booking = db.Bookings
                    .FirstOrDefault(b => b.CarId == id && b.UserId == user.Id && b.Status == "Completed");

                ViewBag.CanReview = booking != null;
            }

            return View(car);
        }


    }
}