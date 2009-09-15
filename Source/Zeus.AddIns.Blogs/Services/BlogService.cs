using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ninject;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.ContentTypes;
using Zeus.FileSystem;
using Zeus.Persistence;
using File=Zeus.FileSystem.File;

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

		public Post AddPost(Blog blog, DateTime dateCreated, string title, string text, string[] categories, bool publish)
		{
			if (dateCreated.Year < 2000)
				dateCreated = DateTime.Now;

			Post post = _contentTypeManager.CreateInstance<Post>(blog);
			post.Title = title;
			post.Text = text;
			post.Created = dateCreated;
			if (publish)
				post.Published = dateCreated;

			post.AddTo(blog);

			PopulateCategories(post, categories);

			_persister.Save(post);

			return post;
		}

		public void UpdatePost(Post post, DateTime dateCreated, string title, string text, string[] categories, bool publish)
		{
			if (dateCreated.Year < 2000)
				dateCreated = DateTime.Now;

			if (!string.IsNullOrEmpty(title))
				post.Title = title;
			if (!string.IsNullOrEmpty(text))
				post.Text = text;
			post.Created = dateCreated;
			post.Published = (publish) ? dateCreated : (DateTime?) null;

			PopulateCategories(post, categories);

			_persister.Save(post);
		}

		private void PopulateCategories(Post post, IEnumerable<string> categories)
		{
			post.Categories.Clear();

			if (categories == null)
				return;

			foreach (string eachCategory in categories)
			{
				Category category = GetOrCreateCategory(post.CurrentBlog, eachCategory);
				post.Categories.Add(category);
			}
		}

		private Category GetOrCreateCategory(Blog blog, string category)
		{
			Category existingCategory = blog.Categories.GetChildren<Category>().SingleOrDefault(c => c.Title == category);
			if (existingCategory != null)
				return existingCategory;

			Category newCategory = _contentTypeManager.CreateInstance<Category>(blog.Categories);
			newCategory.Name = Utility.GetSafeName(category);
			newCategory.Title = category;
			newCategory.AddTo(blog.Categories);
			return newCategory;
		}

		public IEnumerable<Post> GetRecentPosts(Blog blog, int numberOfPosts)
		{
			return Find.EnumerateAccessibleChildren(blog)
				//.Where(p => p.IsPublished())
				.OfType<Post>().OrderByDescending(p => p.Date)
				.Take(numberOfPosts);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="blog"></param>
		/// <param name="name">Filename, which may include a directory path.</param>
		/// <param name="mimeType"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public File AddFile(Blog blog, string name, string mimeType, byte[] data)
		{
			string folder = name.Substring(0, name.LastIndexOf("/") + 1);
			Folder filesFolder = EnsureFolder(blog.Files, folder);

			// If existing file with same name, remove it.
			string filename = name;
			if (filename.Contains("/"))
				filename = filename.Substring(filename.LastIndexOf("/") + 1);
			File existingFile = filesFolder.GetChild(filename) as File;
			if (existingFile != null)
				_persister.Delete(existingFile);

			File newFile = _contentTypeManager.CreateInstance<File>(filesFolder);
			newFile.ContentType = mimeType;
			newFile.Name = filename;
			newFile.Title = filename;
			newFile.Data = data;
			newFile.AddTo(filesFolder);

			_persister.Save(newFile);

			return newFile;
		}

		private Folder EnsureFolder(Folder folder, string folderName)
		{
			Folder currentFolder = folder;

			string[] folderNameParts = folderName.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string folderNamePart in folderNameParts)
			{
				Folder existingFolder = currentFolder.GetChild(folderNamePart) as Folder;
				if (existingFolder != null)
				{
					currentFolder = existingFolder;
					continue;
				}

				Folder newFolder = _contentTypeManager.CreateInstance<Folder>(currentFolder);
				newFolder.Name = folderNamePart;
				newFolder.AddTo(currentFolder);

				currentFolder = newFolder;
			}

			return currentFolder;
		}
	}
}