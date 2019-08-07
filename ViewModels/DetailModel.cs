using SheilaWard_CFBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SheilaWard_CFBlog.ViewModels
{
    public class DetailVM
    {
        public BlogPost blogPost { get; set; }
        public ICollection<BlogPost> recentPosts { get; set; }
        public ICollection<Comment> recentComments { get; set; }
        public IGrouping<BlogPost> archivePosts { get; set; }

        public interface IGrouping<T>
        {
        }
    }
}