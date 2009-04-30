using System;
using System.Collections.Generic;
using System.Diagnostics;
using Isis.Web;

namespace Zeus.Web
{
	/// <summary>
	/// A data carrier used to pass data about a found content item and 
	/// it's template from the content item to the url rewriter.
	/// </summary>
	[Serializable, DebuggerDisplay("PathData ({CurrentItem})")]
	public class PathData
	{
		#region Fields

		public const string DefaultAction = "";

		#endregion

		#region Constructors

		public PathData(ContentItem item, string templateUrl, string action, string arguments)
			: this()
		{
			if (item != null)
			{
				CurrentItem = item;
				ID = item.ID;
			}
			TemplateUrl = templateUrl;
			Action = action;
			Argument = arguments;
		}

		public PathData(ContentItem item, string templateUrl)
			: this(item, templateUrl, DefaultAction, string.Empty)
		{

		}

		public PathData(int id, string path, string templateUrl, string action, string arguments)
			: this()
		{
			ID = id;
			Path = path;
			TemplateUrl = templateUrl;
			Action = action;
			Argument = arguments;
		}

		public PathData()
		{
			QueryParameters = new Dictionary<string, string>();
		}

		#endregion

		#region Properties

		public static string ItemQueryKey = "item";
		public static string PageQueryKey = "page";

		public static PathData Empty
		{
			get { return new PathData(); }
		}

		public ContentItem CurrentItem { get; set; }

		/// <summary>
		/// Gets or sets a flag which indicates whether the page this template refers to needs to be secured with SSL.
		/// </summary>
		public bool SslSecured { get; set; }

		public string TemplateUrl { get; set; }
		public int ID { get; set; }
		public string Path { get; set; }
		public string Action { get; set; }
		public string Argument { get; set; }
		public IDictionary<string, string> QueryParameters { get; set; }
		public bool IsRewritable { get; set; }

		public virtual Url RewrittenUrl
		{
			get
			{
				if (IsEmpty())
					return null;

				string templateUrl = !string.IsNullOrEmpty(TemplateUrl) ? TemplateUrl : "/";
				if (CurrentItem.IsPage)
					return Url.Parse(templateUrl).UpdateQuery(QueryParameters).SetQueryParameter(PageQueryKey, CurrentItem.ID);

				for (ContentItem ancestor = CurrentItem.Parent; ancestor != null; ancestor = ancestor.Parent)
					if (ancestor.IsPage)
						return ancestor.FindPath(DefaultAction).RewrittenUrl.UpdateQuery(QueryParameters).SetQueryParameter(ItemQueryKey, CurrentItem.ID);

				if (CurrentItem.VersionOf != null)
					return CurrentItem.VersionOf.FindPath(DefaultAction).RewrittenUrl.UpdateQuery(QueryParameters).SetQueryParameter(ItemQueryKey, CurrentItem.ID);

				throw new TemplateNotFoundException(CurrentItem);
			}
		}

		public virtual Url Url
		{
			get
			{
				Url result = new Url(CurrentItem.Url);
				if (SslSecured)
					result = result.SetScheme("https").SetAuthority(new Url(Url.ServerUrl).Authority);
				if (!string.IsNullOrEmpty(Action))
					result = result.AppendSegment(Action);
				return result;
			}
		}

		#endregion

		#region Methods

		public virtual PathData UpdateParameters(IDictionary<string, string> queryString)
		{
			foreach (KeyValuePair<string, string> pair in queryString)
				QueryParameters[pair.Key] = pair.Value;
			return this;
		}

		public virtual PathData Detach()
		{
			PathData data = new PathData(ID, Path, TemplateUrl, Action, Argument);
			data.QueryParameters = new Dictionary<string, string>(data.QueryParameters);
			return data;
		}

		public virtual PathData Attach(Persistence.IPersister persister)
		{
			ContentItem item = persister.Repository.Load(ID);
			PathData data = new PathData(item, TemplateUrl, Action, Argument)
    	{
    		QueryParameters = new Dictionary<string, string>(QueryParameters)
    	};
			return data;
		}

		public virtual PathData SetArguments(string argument)
		{
			Argument = argument;
			return this;
		}

		/// <summary>Checks whether the path contains data.</summary>
		/// <returns>True if the path is empty.</returns>
		public virtual bool IsEmpty()
		{
			return CurrentItem == null;
		}

		public static PathData EmptyTemplate()
		{
			return new PathData(null, null, null, null);
		}

		#endregion
	}
}