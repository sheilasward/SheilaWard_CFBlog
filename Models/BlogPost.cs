using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SheilaWard_CFBlog.Models
{
    public class BlogPost
    {
        public BlogPost()
        {
            Comments = new HashSet<Comment>();
        }

        // Id will be assigned to blog post
        public int Id { get; set; }

        // AuthorId will be extracted from ApplicationUser who creates the blog post
        public string AuthorId { get; set; }

        // Created field will be generated at time blog post is created
        public DateTimeOffset Created { get; set; }

        // Updated field will be generated when a blog post is edited
        public DateTimeOffset? Updated { get; set; }

        [Display(Name = "Blog Title")]
        public string Title { get; set; }

        // Slug will be produced behind the scenes using the title
        public string Slug { get; set; }

        // Abstract will be used to display a short snippet to the user
        public string Abstract { get; set; }

        [AllowHtml]
        public string Body { get; set; }

        // MediaURL will eventually have an upload input
        public string MediaURL { get; set; }

        // Published if left unchecked cannot be viewed by public
        public bool Published { get; set; }


        // Navigation properties use 'virtual'
        public virtual ApplicationUser Author { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}