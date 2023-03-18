using Blog_Site.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace Blog_Site.Controllers
{
    public class AdminsController : Controller
    {
        private BlogDbEntities db = new BlogDbEntities();

        // GET: Admins
        public ActionResult Index()
        {
            if (Session["AdminId"] != null)
            {
                IQueryable<Post> post = db.Posts.Where(x => x.PostStatus == null).OrderBy(x => x.PostDateTime);
                return View(post.ToList());
            }
            else
                return RedirectToAction("Login", "Users");
        }

        public ActionResult ManagePost(int? id)
        {
            if (Session["AdminId"] != null)
            {
                Post post = db.Posts.SingleOrDefault(x => x.PostId == id);
                var user = db.Users.Where(u => u.UserId.Equals(post.UserId));            
                ViewBag.username = user.FirstOrDefault().UserName;
                return View(post);
            }
            else
                return RedirectToAction("Login", "Users");
        }

        public ActionResult PostApprove(int id = 0)
        {
            var newpost = db.Posts.Find(id);

            if (TryUpdateModel(newpost))
            {
                newpost.PostStatus = true;
                db.SaveChanges();
                ViewBag.updatesuccess = "Update Successful";
                return RedirectToAction("Index", "Admins");
            }          
            return RedirectToAction("Index", "Admins");
        }

        public ActionResult PostDelete(int? id)
        {
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var newpost = db.Posts.Find(id);
            if (TryUpdateModel(newpost))
            {
                newpost.PostStatus = false;
                db.SaveChanges();
                ViewBag.updatesuccess = "Update Successful";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}