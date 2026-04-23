using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Data;
using WebApplication2.Models;
using System.Data.Entity; 

namespace WebApplication2.Controllers
{
    public class FavoriteController : BaseController
    {
        private CarRentalDbContext db = new CarRentalDbContext();
        public ActionResult Index()
        {
            int userId = Convert.ToInt32(Session["UserId"]);

            var favorites = db.FavoriteCars
                              .Include(f => f.Car)
                              .Where(f => f.UserId == userId)
                              .ToList();

            return View(favorites); // this will load Views/Favorite/Index.cshtml
        }

        [HttpPost]
        public ActionResult Add(int carId)
        {
            int userId = Convert.ToInt32(Session["UserId"]);

            var exists = db.FavoriteCars.FirstOrDefault(f => f.CarId == carId && f.UserId == userId);

            if (exists == null)
            {
                var favorite = new FavoriteCar
                {
                    CarId = carId,
                    UserId = userId,
                    AddedDate = DateTime.Now
                };
                db.FavoriteCars.Add(favorite);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Car added to favorites!";
            }
            else
            {
                TempData["SuccessMessage"] = "Car is already in your favorites.";
            }
            return RedirectToAction("Index", "Favorite");
        }

        public ActionResult Remove(int id)
        {
            var favorite = db.FavoriteCars.Find(id);
            if (favorite != null)
            {
                db.FavoriteCars.Remove(favorite);
                db.SaveChanges();
            }

            TempData["SuccessMessage"] = "Removed from favorites.";
            return RedirectToAction("Dashboard", "Customer");
        }
    }
}