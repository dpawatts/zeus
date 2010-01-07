using System;
using System.Collections.Generic;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.FileSystem;

namespace Zeus.AddIns.Blogs.Services
{
	public interface IBlogService
	{
		Post AddPost(Blog blog, DateTime dateCreated, string title, string text, string[] categories, bool publish);
		void UpdatePost(Post post, DateTime dateCreated, string title, string text, string[] categories, bool publish);
		IEnumerable<Post> GetRecentPosts(Blog blog, int numberOfPosts);
		File AddFile(Blog blog, string name, string mimeType, byte[] data);
		IEnumerable<Post> GetPostsInCategory(Blog blog, Category category);
	}
}