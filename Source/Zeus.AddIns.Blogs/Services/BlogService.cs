using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.ContentTypes;
using Zeus.Persistence;

namespace Zeus.AddIns.Blogs.Services
{
	public class BlogService : IBlogService, IStartable
	{
		#region Fields

		private readonly IPersister _persister;
		private readonly IContentTypeManager _contentTypeManager;

		#endregion

		#region Constructor

		public BlogService(IPersister persister, IContentTypeManager contentTypeManager)
		{
			_persister = persister;
			_contentTypeManager = contentTypeManager;
		}

		#endregion

		#region Methods

		public void Start()
		{
			_persister.ItemSaving += OnPersisterItemSaving;
		}

		private void OnPersisterItemSaving(object sender, CancelItemEventArgs e)
		{
			if (e.AffectedItem is Post && e.AffectedItem.TranslationOf == null)
			{
				// Move blog post to correct year / month, creating those nodes if necessary.
				Post post = (Post)e.AffectedItem;

				// Get or create year item.
				BlogYear year = (BlogYear)post.CurrentBlog.GetChild(post.Date.ToString("yyyy"))
												?? CreateItem<BlogYear>(post.CurrentBlog, post.Date.ToString("yyyy"), post.Date.ToString("yyyy"));

				// Get or create month item.
				BlogMonth month = (BlogMonth)year.GetChild(post.Date.ToString("MM"))
													?? CreateItem<BlogMonth>(year, post.Date.ToString("MM"), post.Date.ToString("MMMM"));

				// Add news item to month.
				post.AddTo(month);
			}
		}

		private T CreateItem<T>(ContentItem parent, string name, string title)
			where T : ContentItem
		{
			T item = _contentTypeManager.CreateInstance<T>(parent);
			item.Name = name;
			item.Title = title;
			item.Visible = false;
			_persister.Save(item);
			return item;
		}

		public void Stop()
		{
			_persister.ItemSaving -= OnPersisterItemSaving;
		}

		#endregion

		public Post AddPost(Blog blog, DateTime dateCreated, string title, string text, bool publish)
		{
			Post post = _contentTypeManager.CreateInstance<Post>(blog);
			post.Title = title;
			post.Text = text;
			post.Created = dateCreated;
			if (publish)
				post.Published = dateCreated;
			
			post.AddTo(blog);
			_persister.Save(post);

			return post;
		}

		public IEnumerable<Post> GetRecentPosts(Blog blog, int numberOfPosts)
		{
			return Find.EnumerateAccessibleChildren(blog)
				//.Where(p => p.IsPublished())
				.OfType<Post>().OrderByDescending(p => p.Date)
				.Take(numberOfPosts);
		}
	}
}