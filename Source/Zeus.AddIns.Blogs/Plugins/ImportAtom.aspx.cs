using System;
using System.Linq;
using Argotic.Syndication;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Admin;

namespace Zeus.AddIns.Blogs.Plugins
{
	[ActionPluginGroup("ImportExportBlogXml", 200)]
	public partial class ImportAtom : PreviewFrameAdminPage
	{
		protected void btnUploadImport_Click(object sender, EventArgs e)
		{
			AtomFeed feed = new AtomFeed();
			feed.Load(uplImport.FileContent);

			Blog blog = (Blog) SelectedItem;

			foreach (AtomEntry entry in feed.Entries.Where(en => en.Categories.Any(c => c.Term == "http://schemas.google.com/blogger/2008/kind#post")))
			{
				Post post = new Post();
				post.Created = post.Updated = entry.UpdatedOn;
				post.Published = entry.PublishedOn;
				post.Text = entry.Content.Content;
				post.Title = entry.Title.Content;
				post.Name = Utility.GetSafeName(post.Title);
				post.AddTo(blog);

				Engine.Persister.Save(post);
			}

			/*foreach (AtomEntry entry in feed.Entries.Where(en => en.Categories.Any(c => c.Term == "http://schemas.google.com/blogger/2008/kind#comment")))
			{
				ISyndicationExtension inReplyTo = entry.FindExtension(se => se.Name == "in-reply-to");
				Comment comment = new Comment();
				comment.Created = comment.Updated = entry.UpdatedOn;
				comment.Title = entry.Title.Content;
				comment.Text = entry.Content.Content;
				comment.Published = entry.PublishedOn;
				if (entry.Authors.Any())
				{
					var author = entry.Authors.First();
					comment.AuthorName = author.Name;
					comment.AuthorUrl = author.Uri.ToString();
					comment.AuthorEmail = author.EmailAddress;
				}
			}*/
		}
	}
}
