using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RavenDBBlogWeb
{
    using Raven.Abstractions.Indexing;
    using Raven.Client.Indexes;

    using RavenDBBlogWeb.Models;

		public class PostsByTagResult
    	{
        	public string Tag { get; set; }
        	public int PostCount { get; set; }
    	}

    	public class Posts_PostsByTag : AbstractIndexCreationTask<Post, PostsByTagResult>
    	{
        	public Posts_PostsByTag()
        	{
            	Map = posts => from post in posts
                           from tag in post.Tags
                	select new PostsByTagResult() { Tag = tag, PostCount = 1 };

            	Reduce = results => results.GroupBy(r => r.Tag)
                	.Select(g => new PostsByTagResult { Tag = g.Key, PostCount = g.Sum(r => r.PostCount) });

                Sort(f => f.PostCount, SortOptions.Int);
        	}
    	}
}