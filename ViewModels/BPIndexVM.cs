using SheilaWard_CFBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SheilaWard_CFBlog.ViewModels
{
    public class BPIndexVM
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ICollection<BlogPost> RecentPosts { get; set; }
        public ICollection<Comment> RecentComments { get; set; }

        // GET: BlogPosts
        [AllowAnonymous]
        public ActionResult Index(int? page, string searchStr)  // The signature is "nullable int" and "string"
        {
            ViewBag.Search = searchStr;
            var blogList = IndexSearch(searchStr);
            var pageSize = 2;
            var pageNumber = page ?? 1;
        }
}