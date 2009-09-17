using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
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
		private readonly IFileSystemService _fileSystemService;

		#endregion

		#region Constructor

		public BlogService(IPersister persister, IContentTypeManager contentTypeManager, IFileSystemService fileSystemService)
		{
			_persister = persister;
			_contentTypeManager = contentTypeManager;
			_fileSystemService = fileSystemService;
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
			Folder filesFolder = _fileSystemService.EnsureFolder(blog.Files, folder);

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

		/// <summary>
		/// Parses some html and returns a string collection of the tag names contained 
		/// within the HTML.
		/// </summary>
		/// <param name="html"></param>
		/// <returns></returns>
		public static List<string> ParseTags(string html)
		{
			Regex relRegex = new Regex(@"\s+rel\s*=\s*(""[^""]*?\btag\b.*?""|'[^']*?\btag\b.*?')", RegexOptions.IgnoreCase | RegexOptions.Singleline);
			Regex hrefRegex = new Regex(@"\s+href\s*=\s*(""(?<url>[^""]*?)""|'(?<url>[^']*?)')", RegexOptions.IgnoreCase | RegexOptions.Singleline);
			Regex anchorRegex = new Regex(@"<a(\s+\w+\s*=\s*(?:""[^""]*?""|'[^']*?')(?!\w))+\s*>.*?</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

			List<string> tags = new List<string>();
			List<string> loweredTags = new List<string>();

			foreach (Match m in anchorRegex.Matches(html))
			{
				string anchorHtml = m.Value;
				if (!relRegex.IsMatch(anchorHtml))
					continue;

				Match urlMatch = hrefRegex.Match(anchorHtml);
				if (urlMatch.Success)
				{
					string urlStr = urlMatch.Groups["url"].Value;
					if (urlStr.EndsWith("/default.aspx", StringComparison.InvariantCultureIgnoreCase))
						urlStr = urlStr.Substring(0, urlStr.Length - 13);
					Uri url;
					if (Uri.TryCreate(urlStr, UriKind.RelativeOrAbsolute, out url))
					{
						string[] seg = url.Segments;
						string tag = HttpUtility.UrlDecode(seg[seg.Length - 1].Replace("/", ""));

						//Keep a list of lowered tags so we can prevent duplicates without modifying capitalization
						string loweredTag = tag.ToLower();
						if (!loweredTags.Contains(loweredTag))
						{
							loweredTags.Add(loweredTag);
							tags.Add(tag);
						}
					}
				}
			}
			return tags;
		}
	}
}