using System;
using CookComputing.XmlRpc;

namespace Zeus.AddIns.Blogs.Web.XmlRpc
{
	/// <summary>
	/// Based on http://codex.wordpress.org/XML-RPC_wp.
	/// </summary>
	public interface IWordPressApi
	{
		[XmlRpcMethod("wp.getUsersBlogs", Description = "Retrieve the blogs of the users. ")]
		WordPressBlogInfo[] GetUsersBlogs(string username, string password);

		[XmlRpcMethod("wp.getTags", Description = "Get list of all tags.")]
		WordPressTag[] GetTags(int blog_id, string username, string password);

		[XmlRpcMethod("wp.getCommentCount", Description = "Retrieve comment count for a specific post.")]
		WordPressCommentCount[] GetCommentCount(int blog_id, string username, string password, string post_id);

		[XmlRpcMethod("wp.getPostStatusList", Description = "Retrieve post statuses.")]
		string[] GetPostStatusList(int blog_id, string username, string password);

		[XmlRpcMethod("wp.getPageStatusList", Description = "Retrieve all of the WordPress supported page statuses.")]
		string[] GetPageStatusList(int blog_id, string username, string password);

		[XmlRpcMethod("wp.getPageTemplates", Description = "Retrieve page templates.")]
		WordPressPageTemplate[] GetPageTemplates(int blog_id, string username, string password);

		[XmlRpcMethod("wp.getOptions", Description = "Retrieve blog options. If passing in an array, search for options listed within it.")]
		WordPressOption[] GetOptions(int blog_id, string username, string password, string[] options);

		[XmlRpcMethod("wp.setOptions", Description = "Update blog options. Returns array of structs showing updated values.")]
		WordPressOption[] SetOptions(int blog_id, string username, string password, WordPressOption[] options);

		[XmlRpcMethod("wp.deleteComment", Description = "Remove comment.")]
		bool DeleteComment(int blog_id, string username, string password, int comment_id);

		[XmlRpcMethod("wp.editComment", Description = "Edit comment.")]
		bool EditComment(int blog_id, string username, string password, int comment_id, WordPressEditComment comment);

		[XmlRpcMethod("wp.newComment", Description = "Create new comment.")]
		int NewComment(int blog_id, string username, string password, int post_id, WordPressNewComment comment);

		[XmlRpcMethod("wp.getCommentStatusList", Description = "Retrieve all of the comment status.")]
		string[] GetCommentStatusList(int blog_id, string username, string password);

		[XmlRpcMethod("wp.getPage", Description = "Get the page identified by the page id.")]
		WordPressPageInfo GetPage(string blog_id, string page_id, string username, string password);

		[XmlRpcMethod("wp.getPages", Description = "Get an array of all the pages on a blog.")]
		WordPressPageInfo[] GetPages(string blog_id, string username, string password);

		[XmlRpcMethod("wp.getPageList", Description = "Get an array of all the pages on a blog. Just the minimum details, lighter than wp.getPages.")]
		WordPressPage[] GetPageList(string blog_id, string username, string password);

		[XmlRpcMethod("wp.newPage", Description = "Create a new page. Similar to metaWeblog.newPost.")]
		int NewPage(string blog_id, string username, string password, Post content, bool publish);

		[XmlRpcMethod("wp.deletePage", Description = "Removes a page from the blog.")]
		bool DeletePage(string blog_id, string username, string password, string page_id);

		[XmlRpcMethod("wp.editPage", Description = "Make changes to a blog page.")]
		bool EditPage(string blog_id, string page_id, string username, string password, Post content, bool publish);

		[XmlRpcMethod("wp.getAuthors", Description = "Get an array of users for the blog.")]
		WordPressAuthor[] GetAuthors(string blog_id, string username, string password);

		[XmlRpcMethod("wp.getCategories", Description = "Get an array of available categories on a blog.")]
		WordPressCategoryInfo[] GetCategories(string blog_id, string username, string password);

		[XmlRpcMethod("wp.newCategory", Description = "Create a new category.")]
		int NewCategory(string blog_id, string username, string password, WordPressCategory category);

		[XmlRpcMethod("wp.deleteCategory", Description = "Deletes a category.")]
		bool DeleteCategory(string blog_id, string username, string password, int category_id);

		[XmlRpcMethod("wp.suggestCategories", Description = "Get an array of categories that start with a given string.")]
		WordPressSuggestCategory[] SuggestCategories(string blog_id, string username, string password, string category, int max_results);

		[XmlRpcMethod("wp.uploadFile", Description = "Upload a file.")]
		MediaObjectInfo UploadFile(string blog_id, string username, string password, MediaObject data);

		[XmlRpcMethod("wp.getComment", Description = "Gets a comment, given it's comment ID. Note that this isn't in 2.6.1, but is in the HEAD (so should be in anything newer than 2.6.1)")]
		WordPressCommentInfo GetComment(string blog_id, string username, string password, int comment_id);

		[XmlRpcMethod("wp.getComments", Description = "Gets a set of comments for a given post.")]
		WordPressCommentInfo[] GetComments(string blog_id, string username, string password, WordPressCommentRequest options);
	}

	public struct WordPressCategory
	{
		public string name;
		public string slug;
		public int parent_id;
		public string description;
	}

	public struct WordPressBlogInfo
	{
		public bool is_admin;
		public string url;
		public int blog_id;
		public string blog_name;
		public string xmlrpc_url;
	}

	public struct WordPressTag
	{
		public int tag_id;
		public string name;
		public int count;
		public string slug;
		public string html_url;
		public string rss_url;
	}

	public struct WordPressCommentCount
	{
		public int approved;
		public int awaiting_moderation;
		public int spam;
		public int total_comments;
	}

	public struct WordPressPageTemplate
	{
		public string name;
		public string description;
	}

	public struct WordPressOption
	{
		public string option;
		public string value;
	}

	public struct WordPressEditComment
	{
		public string status;
		public DateTime date_created_gmt;
		public string content;
		public string author;
		public string author_url;
		public string author_email;
	}

	public struct WordPressNewComment
	{
		public int comment_parent;
		public string content;
		public string author;
		public string author_url;
		public string author_email;
	}

	public struct WordPressPageInfo
	{
		public DateTime dateCreated;
		public int userid;
		public int page_id;
		public string page_status;
		public string description;
		public string title;
		public string link;
		public string permalink;
		public string[] categories; // probably not right
		public string excerpt;
		public string text_more;
		public int mt_allow_comments;
		public int mt_allow_pings;
		public string wp_slug;
		public string wp_password;
		public string wp_author;
		public int wp_page_parent_id;
		public string wp_page_parent_title;
		public int wp_page_order;
		public int wp_author_id;
		public string wp_author_display_name;
		public DateTime date_created_gmt;
		public WordPressPageCustomField[] custom_fields;
		public string wp_page_template;
	}

	public struct WordPressPageCustomField
	{
		public string id;
		public string key;
		public string value;
	}

	public struct WordPressPage
	{
		public int page_id;
		public string page_title;
		public int page_parent_id;
		public DateTime dateCreated;
	}

	public struct WordPressAuthor
	{
		public int user_id;
		public string user_login;
		public string display_name;
		public string user_email;

		/// <summary>
		/// Serialized PHP data.
		/// </summary>
		public string meta_value;
	}

	public struct WordPressCategoryInfo
	{
		public int categoryId;
		public int parentId;
		public string description;
		public string categoryName;
		public string htmlUrl;
		public string rssUrl;
	}

	public struct WordPressSuggestCategory
	{
		public int category_id;
		public string category_name;
	}

	public struct WordPressCommentInfo
	{
		public DateTime dateCreated;
		public string user_id;
		public string comment_id;
		public string parent;
		public string status;
		public string content;
		public string link;
		public string post_id;
		public string post_title;
		public string author;
		public string author_url;
		public string author_email;
		public string author_ip;
	}

	public struct WordPressCommentRequest
	{
		public string post_id;
		public string status;
		public string offset;
		public string number;
	}
}