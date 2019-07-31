using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SheilaWard_CFBlog.Helpers;
using SheilaWard_CFBlog.Models;
using PagedList;
using PagedList.Mvc;
using SheilaWard_CFBlog.ViewModels;

namespace SheilaWard_CFBlog.Controllers
{
    [Authorize(Roles = "Admin")]
    [RequireHttps]
    public class BlogPostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BlogPosts
        [AllowAnonymous]
        public ActionResult Index(int? page, string searchStr)  // The signature is "nullable int" and "string"
        {
            ViewBag.Search = searchStr;
            var blogList = IndexSearch(searchStr);
            var pageSize = 2;
            var pageNumber = page ?? 1;

            //var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            //var user = userManager.FindByName(User.Identity.Name);
            //if (user != null && user.Email == "admin@myblog.com")
            //{
            //    return View(db.Posts.OrderByDescending(b => b.Created).ToPagedList(pageNumber, pageSize));
            //}
            //else
            //{
            //    return View(db.Posts.Where(b => b.Published).OrderByDescending(b => b.Created).ToPagedList(pageNumber, pageSize));
            //}
            return View(blogList.ToPagedList(pageNumber, pageSize));
        }

        public IQueryable<BlogPost> IndexSearch(string searchStr)
        {
            var result = db.Posts.AsQueryable();
            if (searchStr != null)
            {
                result = result.Where(p => p.Title.Contains(searchStr) ||
                                           p.Body.Contains(searchStr) ||
                                           p.Comments.Any(c => c.CommentBody.Contains(searchStr) ||
                                                               c.Author.FirstName.Contains(searchStr) ||
                                                               c.Author.LastName.Contains(searchStr) ||
                                                               c.Author.DisplayName.Contains(searchStr) ||
                                                               c.Author.Email.Contains(searchStr)));
            }
            return result.OrderByDescending(p => p.Created);
        }

        // GET: BlogPosts/Details/5
        [AllowAnonymous]
        public ActionResult Details(string slug)
        {
            if (String.IsNullOrWhiteSpace(slug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.FirstOrDefault(p => p.Slug == slug);
            if (blogPost == null)
            {
                return HttpNotFound();
            }

            var detailVM = new DetailVM();
            detailVM.blogPost = blogPost;
            detailVM.recentPosts = db.Posts.Where(p => p.Published && slug != p.Slug).OrderByDescending(c => c.Created).Take(3).ToList();
            detailVM.recentComments = db.Comments.Where(c => c.CommentBody != null).OrderByDescending(c => c.Created).Take(4).ToList();
            //detailVM.archivePosts = db.Posts.Where(p => p.Published).OrderByDescending(c => c.Created).GroupBy(c => c.Created.ToString("MMM/yyyy")).ToList();


            return View(detailVM);
        }


        // GET: BlogPosts/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "DisplayName");
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Abstract,Body,MediaURL,Published,AuthorId")] BlogPost blogPost, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                    blogPost.MediaURL = "/Uploads/" + fileName;
                }

                var slug = StringUtilities.URLFriendly(blogPost.Title);
                if (String.IsNullOrWhiteSpace(slug))
                {
                    ModelState.AddModelError("Title", "Invalid title");
                    return View(blogPost);
                }
                if (db.Posts.Any(p => p.Slug == slug))
                {
                    ModelState.AddModelError("Title", "The title must be unique");
                    return View(blogPost);
                }
                blogPost.Slug = slug;

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var user = userManager.FindByName(User.Identity.Name);
                string userId = user.Id;
                blogPost.AuthorId = user.Id;

                blogPost.Created = DateTimeOffset.Now;
                
                db.Posts.Add(blogPost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5
        public ActionResult Edit(string slug)
        {
            if (String.IsNullOrWhiteSpace(slug)) 
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.FirstOrDefault(p => p.Slug == slug);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "DisplayName", blogPost.AuthorId);
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Slug,Created,Abstract,Body,MediaURL,Published")] BlogPost blogPost, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                    blogPost.MediaURL = "/Uploads/" + fileName;
                }

                var slug = StringUtilities.URLFriendly(blogPost.Title);
                if (blogPost.Slug != slug)
                {
                    if (String.IsNullOrWhiteSpace(slug))
                    {
                        ModelState.AddModelError("Title", "Invalid title");
                        return View(blogPost);
                    }
                    if (db.Posts.Any(p => p.Slug == slug))
                    {
                        ModelState.AddModelError("Title", "The title must be unique");
                        return View(blogPost);
                    }

                    blogPost.Slug = slug;
                }

                blogPost.Updated = DateTimeOffset.Now;
                db.Entry(blogPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "DisplayName", blogPost.AuthorId);
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost blogPost = db.Posts.Find(id);
            db.Posts.Remove(blogPost);
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
    }
}
