using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class UserController :  BaseController
    {
        private CarRentalDbContext db = new CarRentalDbContext();

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(user);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                Session["UserId"] = user.Id;
                Session["UserEmail"] = user.Email;
                Session["UserRole"] = user.Role;

                if (user.Role == "Admin")
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    return RedirectToAction("Dashboard", "Customer");
                }
            }

            ViewBag.Error = "Invalid email or password.";
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();

            // Expire the authentication cookie
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                var cookie = new HttpCookie("ASP.NET_SessionId", "");
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Login", "User");
        }
        public ActionResult ManageUsers()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
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
    }
}

