using SheilaWard_CFBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SheilaWard_CFBlog.Helpers
{
    public class DataUtilities
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        public static List<Comment> GetComments()
        {
            return db.Comments.OrderByDescending(c => c.Created).Take(4).ToList();

        }
    }
}