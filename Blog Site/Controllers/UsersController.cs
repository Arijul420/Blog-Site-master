using Blog_Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog_Site.Controllers
{
    public class UsersController : Controller
    {
        private BlogDbEntities db = new BlogDbEntities();

        // GET: Users
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();

                return Content("Sign Up Successfull");
                //return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Users
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(TempUser tempUser)
        {
            if (ModelState.IsValid)
            {
                var admin = db.Admins.Where(a => a.AdminName.Equals(tempUser.UserName) && a.AdminPassword.Equals(tempUser.UserPassword)).FirstOrDefault();
                var user = db.Users.Where(u => u.UserName.Equals(tempUser.UserName) && u.UserPassword.Equals(tempUser.UserPassword)).FirstOrDefault();

                if (admin != null)
                {
                    Session["AdminId"] = admin.AdminId;
                    return RedirectToAction("Index", "Admins");
                }
                else if (user != null)
                {
                    Session["UserId"] = user.UserId;
                    return RedirectToAction("Index", "Home");
                }
                else
                    return Content("Sign In Failed");
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Users");
        }

    }
}