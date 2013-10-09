using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RavenDBBlogWeb.Controllers
{
    using Raven.Client;

    using RavenDBBlogWeb.Models;

    public class BlogsController : Controller
    {
        private readonly IDocumentSession _session;

        public BlogsController(IDocumentSession session)
        {
            _session = session;
        }

        //
        // GET: /Blogs/

        public ActionResult Index()
        {
            var blogs = _session.Query<Blog>().ToList();
            return View(blogs);
        }

        public ActionResult PostsByTag()
        {
            var results = _session.Query<PostsByTagResult, Posts_PostsByTag>();
            return View(results);
        }
    }
}
