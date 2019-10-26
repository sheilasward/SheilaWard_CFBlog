using SheilaWard_CFBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SheilaWard_CFBlog.Helpers
{
    public class BlogPostHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //public IEnumerable<IGrouping<string, List<BlogPost>>> BlogsByMonth()
        //{
        //    var blogs = new List<BlogPost>();
        //    return null;

        //    //return blogs.GroupBy(c => c.Created.ToString("yyyy MMM"));

        //}

        public static IEnumerable<IGrouping<string, BlogPost>> BlogsByMonth()
        {
            var blogs = DataUtilities.GetBlogPosts();

            IEnumerable<IGrouping<string, BlogPost>> query = blogs
                .OrderByDescending(b => b.Created)
                .GroupBy(b => b.Created.ToString("MMM yyyy"))
                .Select(b => b);
            //select new { Month = mg.Key, Orders = mg}
            //blogs.GroupBy(x => new { x.Created.Year, x.Created.Month });

            return query;


        }


    }
}