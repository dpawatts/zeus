using System;
using System.Collections.Generic;
using Zeus.AddIns.Blogs.ContentTypes;

namespace Zeus.AddIns.Blogs.Services
{
	public interface IBlogService
	{
		Post AddPost(Blog blog, DateTime dateCreated, string title, string text, bool publish);
		IEnumerable<Post> GetRecentPosts(Blog blog, int numberOfPosts);
	}
}