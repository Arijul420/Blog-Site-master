using Blog_Site.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Blog_Site.Controllers
{
    public class HomeController : Controller
    {
        private BlogDbEntities db = new BlogDbEntities();

        public ActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                IQueryable<Post> post = db.Posts.Where(x => x.PostStatus == true);
                return View(post.ToList());

            }
            else
            {
                IQueryable<Post> post = db.Posts.Where(x => x.PostStatus == true && (x.PostTitle.Contains(search) || x.PostTopic.Contains(search)));
                return View(post.ToList());
            }           
        }

        public ActionResult CreatePost()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
                return RedirectToAction("Login", "Users");
        }

        [HttpPost]
        public ActionResult CreatePost(Post post)
        {
            if (Session["UserId"] != null)
            {
                post.UserId = Convert.ToInt32(Session["UserId"].ToString());
                post.PostDateTime = DateTime.Now;
                db.Posts.Add(post);
                db.SaveChanges();
                ViewBag.SuccessMessage = "Post Added";
                //return View("CreatePost", new Post());
                return RedirectToAction("Dashboard");
            }
            else
                return RedirectToAction("Login", "Users");

        }

        public ActionResult PostDetails(int? id)
        {
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null){
                return HttpNotFound();
            }
            return View(post);
        }

        public ActionResult Dashboard()
        {
            if (Session["UserId"] != null)
            {
                var uid = Convert.ToInt32(Session["UserId"].ToString());
                IQueryable<Post> post = db.Posts.Where(x => x.PostStatus == null && x.UserId==uid);

                List<Post> publishedPost = db.Posts.Where(p => p.PostStatus == true && p.UserId == uid).ToList();
                ViewBag.publishedPost = publishedPost;

                return View(post.ToList());
            }
            else
                return RedirectToAction("Login", "Users");
        }

        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost([Bind(Include = "PostId,UserId,PostTitle,PostTopic,PostDetails")] Post post)
        {
            post.PostDateTime = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Welcome to Blog Site, the social network for those interested Participative Technologies in the classroom. We encourage you to sign up to participate in the great discussions here, to receive event notifications, and to find and connect with colleagues.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Blog Site contact page.";

            return View();
        }
    }
}