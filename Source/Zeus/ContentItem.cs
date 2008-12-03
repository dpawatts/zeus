﻿using System;
using Zeus.Integrity;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using Zeus.Web;
using Zeus.Security;

namespace Zeus
{
	[RestrictParents(typeof(ContentItem))]
	public abstract class ContentItem : IUrlParserDependency
	{
		#region Private Fields

		private IList<AuthorizedRole> _authorizedRoles;
		private string _name;
		private string _url;
		private DateTime? _published = DateTime.Now;
		private DateTime? _expires = null;
		private IList<ContentItem> _children = new List<ContentItem>();
		private IDictionary<string, Details.ContentDetail> _details = new Dictionary<string, Details.ContentDetail>();
		private IDictionary<string, Details.DetailCollection> _detailCollections = new Dictionary<string, Details.DetailCollection>();
		private IUrlParser _urlParser;

		#endregion

		#region Public Properties (persisted)

		/// <summary>Gets or sets item ID.</summary>
		public virtual int ID
		{
			get;
			set;
		}

		/// <summary>Gets or sets this item's parent. This can be null for root items and previous versions but should be another page in other situations.</summary>
		public virtual ContentItem Parent
		{
			get;
			set;
		}

		/// <summary>Gets or sets the item's title. This is used in edit mode and probably in a custom implementation.</summary>
		public virtual string Title
		{
			get;
			set;
		}

		/// <summary>Gets or sets the item's name. This is used to compute the item's url and can be used to uniquely identify the item among other items on the same level.</summary>
		public virtual string Name
		{
			get
			{
				return _name ?? (ID > 0 ? ID.ToString() : null);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
					_name = null;
				else
					_name = value;
				_url = null;
			}
		}

		/// <summary>Gets or sets zone name which is associated with data items and their placement on a page.</summary>
		public virtual string ZoneName
		{
			get;
			set;
		}

		/// <summary>Gets or sets when this item was initially created.</summary>
		public virtual DateTime Created
		{
			get;
			set;
		}

		/// <summary>Gets or sets the date this item was updated.</summary>
		public virtual DateTime Updated
		{
			get;
			set;
		}

		/// <summary>Gets or sets the publish date of this item.</summary>
		public virtual DateTime? Published
		{
			get { return _published; }
			set { _published = value; }
		}

		/// <summary>Gets or sets the expiration date of this item.</summary>
		public virtual DateTime? Expires
		{
			get { return _expires; }
			set { _expires = value != DateTime.MinValue ? value : null; }
		}

		/// <summary>Gets or sets the sort order of this item.</summary>
		public virtual int SortOrder
		{
			get;
			set;
		}

		/// <summary>Gets or sets whether this item is visible. This is normally used to control it's visibility in the site map provider.</summary>
		public virtual bool Visible
		{
			get;
			set;
		}

		/// <summary>Gets or sets the published version of this item. If this value is not null then this item is a previous version of the item specified by VersionOf.</summary>
		public virtual ContentItem VersionOf
		{
			get;
			set;
		}

		/// <summary>Gets or sets the name of the identity who saved this item.</summary>
		public virtual string SavedBy
		{
			get;
			set;
		}

		/// <summary>Gets or sets the details collection. These are usually accessed using the e.g. item["Detailname"]. This is a place to store content data.</summary>
		public virtual IDictionary<string, Details.ContentDetail> Details
		{
			get { return _details; }
			set { _details = value; }
		}

		/// <summary>Gets or sets the details collection collection. These are details grouped into a collection.</summary>
		public virtual IDictionary<string, Details.DetailCollection> DetailCollections
		{
			get { return _detailCollections; }
			set { _detailCollections = value; }
		}

		/// <summary>Gets or sets all a collection of child items of this item ignoring permissions. If you want the children the current user has permission to use <see cref="GetChildren()"/> instead.</summary>
		public virtual IList<ContentItem> Children
		{
			get { return _children; }
			set { _children = value; }
		}

		#endregion

		#region Public Properties (generated)

		/// <summary>Gets whether this item is a page. This is used for and site map purposes.</summary>
		public virtual bool IsPage
		{
			get { return true; }
		}

		/// <summary>Gets the template that handle the presentation of this content item. For non page items (IsPage) this can be a user control (ascx).</summary>
		public virtual string TemplateUrl
		{
			get { return "~/default.aspx"; }
		}

		/// <summary>Gets the icon of this item. This can be used to distinguish item types in edit mode.</summary>
		public virtual string IconUrl
		{
			get { return VirtualPathUtility.ToAbsolute("~/edit/img/ico/" + (IsPage ? "page.gif" : "page_white.gif")); }
		}

		/// <summary>Gets the non-friendly url to this item (e.g. "/default.aspx?page=1"). This is used to uniquely identify this item when rewriting to the template page. Non-page items have two query string properties; page and item (e.g. "/default.aspx?page=1&amp;item&#61;27").</summary>
		public virtual string RewrittenUrl
		{
			get
			{
				if (IsPage)
				{
					return new Uri(VirtualPathUtility.ToAbsolute(TemplateUrl)).AppendQuery("page", ID).ToString();
				}

				for (ContentItem ancestorItem = Parent; ancestorItem != null; ancestorItem = ancestorItem.Parent)
					if (ancestorItem.IsPage)
						return new Uri(VirtualPathUtility.ToAbsolute(ancestorItem.TemplateUrl)).AppendQuery("page", ancestorItem.ID).AppendQuery("item", ID).ToString();

				if (VersionOf != null)
					return VersionOf.TemplateUrl;

				throw new TemplateNotFoundException(this);
			}
		}

		/// <summary>Gets the public url to this item. This is computed by walking the parent path and prepending their names to the url.</summary>
		public virtual string Url
		{
			get
			{
				return _url ?? (_url =
					(_urlParser != null && VersionOf == null)
						? _urlParser.BuildUrl(this)
						: RewrittenUrl);
			}
		}

		#endregion

		/// <summary>Gets an array of roles allowed to read this item. Null or empty list is interpreted as this item has no access restrictions (anyone may read).</summary>
		public virtual IList<AuthorizedRole> AuthorizedRoles
		{
			get
			{
				if (_authorizedRoles == null)
					_authorizedRoles = new List<Security.AuthorizedRole>();
				return _authorizedRoles;
			}
			set { _authorizedRoles = value; }
		}

		#region this[]

		/// <summary>Gets or sets the detail or property with the supplied name. If a property with the supplied name exists this is always returned in favour of any detail that might have the same name.</summary>
		/// <param name="detailName">The name of the propery or detail.</param>
		/// <returns>The value of the property or detail. If now property exists null is returned.</returns>
		public virtual object this[string detailName]
		{
			get
			{
				if (detailName == null)
					throw new ArgumentNullException("detailName");

				switch (detailName)
				{
					case "ID":
						return ID;
					case "Title":
						return Title;
					case "Name":
						return Name;
					case "Url":
						return Url;
					case "TemplateUrl":
						return TemplateUrl;
					default:
						return Utility.Evaluate(this, detailName)
							?? GetDetail(detailName)
							?? GetDetailCollection(detailName, false);
				}
			}
			set
			{
				if (string.IsNullOrEmpty(detailName))
					throw new ArgumentNullException("Parameter 'detailName' cannot be null or empty.", "detailName");

				PropertyInfo info = GetType().GetProperty(detailName);
				if (info != null && info.CanWrite)
				{
					if (value != null && info.PropertyType != value.GetType())
						value = Utility.Convert(value, info.PropertyType);
					info.SetValue(this, value, null);
				}
				else if (value is Details.DetailCollection)
					throw new ZeusException("Cannot set a detail collection this way, add it to the DetailCollections collection instead.");
				else
				{
					SetDetail(detailName, value);
				}
			}
		}
		#endregion

		public ContentItem()
		{
			this.Created = DateTime.Now;
			this.Updated = DateTime.Now;
			this.Published = DateTime.Now;
		}

		#region GetDetail & SetDetail<T> Methods

		/// <summary>Gets a detail from the details bag.</summary>
		/// <param name="detailName">The name of the value to get.</param>
		/// <returns>The value stored in the details bag or null if no item was found.</returns>
		public virtual object GetDetail(string detailName)
		{
			return Details.ContainsKey(detailName)
				? Details[detailName].Value
				: null;
		}

		/// <summary>Gets a detail from the details bag.</summary>
		/// <param name="detailName">The name of the value to get.</param>
		/// <param name="defaultValue">The default value to return when no detail is found.</param>
		/// <returns>The value stored in the details bag or null if no item was found.</returns>
		public virtual T GetDetail<T>(string detailName, T defaultValue)
		{
			return Details.ContainsKey(detailName)
				? (T) Details[detailName].Value
				: defaultValue;
		}

		/// <summary>Set a value into the <see cref="Details"/> bag. If a value with the same name already exists it is overwritten. If the value equals the default value it will be removed from the details bag.</summary>
		/// <param name="detailName">The name of the item to set.</param>
		/// <param name="value">The value to set. If this parameter is null or equal to defaultValue the detail is removed.</param>
		/// <param name="defaultValue">The default value. If the value is equal to this value the detail will be removed.</param>
		protected virtual void SetDetail<T>(string detailName, T value, T defaultValue)
		{
			if (value == null || !value.Equals(defaultValue))
			{
				SetDetail<T>(detailName, value);
			}
			else if (Details.ContainsKey(detailName))
			{
				_details.Remove(detailName);
			}
		}

		/// <summary>Set a value into the <see cref="Details"/> bag. If a value with the same name already exists it is overwritten.</summary>
		/// <param name="detailName">The name of the item to set.</param>
		/// <param name="value">The value to set. If this parameter is null the detail is removed.</param>
		protected virtual void SetDetail<T>(string detailName, T value)
		{
			Details.ContentDetail detail = Details.ContainsKey(detailName) ? Details[detailName] : null;

			if (detail != null && value != null && typeof(T).IsAssignableFrom(detail.ValueType))
			{
				// update an existing detail
				detail.Value = value;
			}
			else
			{
				if (detail != null)
					// delete detail or remove detail of wrong type
					Details.Remove(detailName);
				if (value != null)
					// add new detail
					Details.Add(detailName, Zeus.Details.ContentDetail.New(this, detailName, value));
			}
		}

		#endregion

		#region GetDetailCollection

		/// <summary>Gets a named detail collection.</summary>
		/// <param name="collectionName">The name of the detail collection to get.</param>
		/// <param name="createWhenEmpty">Wether a new collection should be created if none exists. Setting this to false means null will be returned if no collection exists.</param>
		/// <returns>A new or existing detail collection or null if the createWhenEmpty parameter is false and no collection with the given name exists..</returns>
		public virtual Details.DetailCollection GetDetailCollection(string collectionName, bool createWhenEmpty)
		{
			if (DetailCollections.ContainsKey(collectionName))
				return DetailCollections[collectionName];
			else if (createWhenEmpty)
			{
				Details.DetailCollection collection = new Details.DetailCollection(this, collectionName);
				DetailCollections.Add(collectionName, collection);
				return collection;
			}
			else
				return null;
		}

		#endregion

		#region Methods

		private const int SORT_ORDER_THRESHOLD = 9999;

		/// <summary>Adds an item to the children of this item updating it's parent refernce.</summary>
		/// <param name="newParent">The new parent of the item. If this parameter is null the item is detached from the hierarchical structure.</param>
		public virtual void AddTo(ContentItem newParent)
		{
			if (Parent != null && Parent != newParent && Parent.Children.Contains(this))
				Parent.Children.Remove(this);

			Parent = newParent;

			if (newParent != null && !newParent.Children.Contains(this))
			{
				IList<ContentItem> siblings = newParent.Children;
				if (siblings.Count > 0)
				{
					int lastOrder = siblings[siblings.Count - 1].SortOrder;

					for (int i = siblings.Count - 2; i >= 0; i--)
					{
						if (siblings[i].SortOrder < lastOrder - SORT_ORDER_THRESHOLD)
						{
							siblings.Insert(i + 1, this);
							return;
						}
						lastOrder = siblings[i].SortOrder;
					}

					if (lastOrder > SORT_ORDER_THRESHOLD)
					{
						siblings.Insert(0, this);
						return;
					}
				}

				siblings.Add(this);
			}
		}

		#endregion

		void IUrlParserDependency.SetUrlParser(IUrlParser parser)
		{
			_urlParser = parser;
		}
	}
}
