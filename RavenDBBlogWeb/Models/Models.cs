using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RavenDBBlogWeb.Models
{

    public class Blog
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
    }

    public class Post
    {
        public string Id { get; set; }
        public string BlogId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<Comment> Comments { get; set; }
        public List<string> Tags { get; set; }
    }

    public class Comment
    {
        public string CommenterName { get; set; }
        public string Text { get; set; }
    }
}