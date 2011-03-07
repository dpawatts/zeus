using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Zeus.ContentTypes;

namespace Zeus.Web.UI
{
	public static class ItemUtility
	{
		public static T FindInParents<T>(Control parentControl)
			where T : class
		{
			if (parentControl == null || parentControl is T)
				return parentControl as T;
			return FindInParents<T>(parentControl.Parent);
		}

		public static Control AddUserControl(Control container, ContentItem item)
		{
			using (new ItemStacker(item))
			{
				return AddTo(item, container);
			}
		}

		private static Control AddTo(ContentItem item, Control container)
		{
			PathData path = item.FindPath(PathData.DefaultAction);
			if (!path.IsEmpty())
			{
				Control templateItem = container.Page.LoadControl(path.TemplateUrl);
				if (templateItem is IContentTemplate)
					(templateItem as IContentTemplate).CurrentItem = item;
				container.Controls.Add(templateItem);
				return templateItem;
			}
			return null;
		}

		public static T EnsureType<T>(ContentItem item)
			where T : ContentItem
		{
			if (item != null && !(item is T))
			{
				throw new ZeusException("Cannot cast the current page " + item
					+ " from type '" + item.GetType()
					+ "' to type '" + typeof(T)
					+ "' required by this template. It might help to change the generic argument of the template to something less explicit (like Zeus.ContentItem) or moving a user control to a page with the correct type or overriding the TemplateUrl property and referencing a more specific template.");
			}
			return (T) item;
		}

		private class ItemStacker : IDisposable
		{
			public ItemStacker(ContentItem currentItem)
			{
				ItemStack.Push(currentItem);
			}
			public void Dispose()
			{
				ItemStack.Pop();
			}
		}

		internal static ContentItem CurrentContentItem
		{
			get { return ItemStack.Peek(); }
		}

		internal static Stack<ContentItem> ItemStack
		{
			get
			{
				Stack<ContentItem> stack = HttpContextItems["ItemStack"] as Stack<ContentItem>;
				if (stack == null)
				{
					HttpContextItems["ItemStack"] = stack = new Stack<ContentItem>();
					stack.Push(Find.CurrentPage);
				}
				return stack;
			}
		}

		internal static IDictionary HttpContextItems
		{
			get { return HttpContext.Current.Items; }
		}

		public static ContentItem WalkPath(ContentItem startItem, string path)
		{
			// find starting point
			if (path.StartsWith("/"))
			{
				startItem = Context.Current.Persister.Get(Context.Current.Host.CurrentSite.RootItemID);
				path = path.Substring(1);
			}
			else if (path.StartsWith("~/"))
			{
				startItem = Context.Current.UrlParser.StartPage;
				path = path.Substring(2);
			}

			// walk path
			ContentItem item = startItem;
			string[] names = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string name in names)
			{
				if (item == null)
					break;
				else if (name == "..")
					item = item.Parent;
				else if (name != "." && !string.IsNullOrEmpty(name))
					item = item.GetChild(name);
			}
			return item;
		}
	}
}
