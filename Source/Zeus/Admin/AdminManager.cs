﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using Zeus.Admin.Plugins;
using Zeus.BaseLibrary.Reflection;
using Zeus.BaseLibrary.Web;
using Zeus.Configuration;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using Zeus.Engine;
using Zeus.Globalization;
using Zeus.Linq;
using Zeus.Persistence;
using Zeus.Security;
using Zeus.Web;
using Zeus.Web.Hosting;
using Zeus.Web.Security;
using Zeus.Web.UI.WebControls;

namespace Zeus.Admin
{
	/// <summary>
	/// Class responsible for plugins in edit mode, knowling links to edit 
	/// pages and saving interaction.
	/// </summary>
	public class AdminManager : IAdminManager
	{
		#region Fields

		private readonly AdminSection _configSection;
		private readonly ISecurityManager _securityManager;
		private readonly IAdminAssemblyManager _adminAssembly;
		private readonly IAuthorizationService _authorizationService;
		private readonly IAuthenticationContextService _authenticationContextService;
		private readonly IPersister _persister;
		private readonly IVersionManager _versionManager;
		private readonly IContentTypeManager _contentTypeManager;
		private readonly Web.IWebContext _webContext;
		private readonly ILanguageManager _languageManager;

		private readonly IEnumerable<ActionPluginGroupAttribute> _cachedActionPluginGroups;

		#endregion

		#region Constructor

		public AdminManager(AdminSection configSection, ISecurityManager securityManager, IAdminAssemblyManager adminAssembly,
			IAuthorizationService authorizationService, IAuthenticationContextService authenticationContextService,
			IPersister persister, IVersionManager versionManager, IContentTypeManager contentTypeManager,
			Web.IWebContext webContext, ILanguageManager languageManager,
			IPluginFinder<ActionPluginGroupAttribute> actionPluginGroupFinder,
			ITypeFinder typeFinder, IEmbeddedResourceManager embeddedResourceManager)
		{
			_configSection = configSection;
			_securityManager = securityManager;
			_adminAssembly = adminAssembly;
			DeleteItemUrl = embeddedResourceManager.GetServerResourceUrl(adminAssembly.Assembly, "Zeus.Admin.Delete.aspx");
			EditItemUrl = embeddedResourceManager.GetServerResourceUrl(adminAssembly.Assembly, "Zeus.Admin.Plugins.EditItem.Default.aspx");
			NewItemUrl = embeddedResourceManager.GetServerResourceUrl(adminAssembly.Assembly, "Zeus.Admin.New.aspx");
			EnableVersioning = configSection.Versioning.Enabled;
			_authorizationService = authorizationService;
			_authenticationContextService = authenticationContextService;
			_persister = persister;
			_versionManager = versionManager;
			_contentTypeManager = contentTypeManager;
			_webContext = webContext;
			_languageManager = languageManager;

			_cachedActionPluginGroups = actionPluginGroupFinder.GetPlugins().OrderBy(g => g.SortOrder);
		}

		#endregion

		#region Events

		/// <summary>Occurs when a version is about to be saved.</summary>
		public event EventHandler<CancelItemEventArgs> SavingVersion;

		#endregion

		#region Properties

		public string DeleteItemUrl { get; set; }
		public string EditItemUrl { get; set; }
		public string NewItemUrl { get; set; }

		public bool EnableVersioning { get; set; }

		public string CurrentAdminLanguageBranch
		{
			get
			{
				return _webContext.Request.QueryString["language"] ??
					((_webContext.Request.Cookies["editlanguagebranch"] != null)
						? _webContext.Request.Cookies["editlanguagebranch"].Value
						: _languageManager.GetDefaultLanguage());
			}
			set
			{
				_webContext.Response.Cookies["editlanguagebranch"].Value = value;
				_webContext.Request.Cookies["editlanguagebranch"].Value = value;
			}
		}

		public bool TreeTooltipsEnabled
		{
			get { return (_configSection.Tree == null || _configSection.Tree.TooltipsEnabled); }
		}

		#endregion

		#region Methods

		/// <summary>Gets the url to the delete item page.</summary>
		/// <param name="selectedItem">The currently selected item.</param>
		/// <returns>The url to the delete page.</returns>
		public string GetDeleteUrl(ContentItem selectedItem)
		{
			return FormatSelectedUrl(selectedItem, DeleteItemUrl);
		}

		/// <summary>Gets the url to the select type of item to create.</summary>
		/// <param name="selectedItem">The currently selected item.</param>
		/// <returns>The url to the select new item to create page.</returns>
		public string GetSelectNewItemUrl(ContentItem selectedItem)
		{
			return FormatSelectedUrl(selectedItem, NewItemUrl);
		}

		/// <summary>Gets the url to the select type of item to create.</summary>
		/// <param name="selectedItem">The currently selected item.</param>
		/// <returns>The url to the select new item to create page.</returns>
		public string GetSelectNewItemUrl(ContentItem selectedItem, string zoneName)
		{
			return FormatSelectedUrl(selectedItem, NewItemUrl + "?zoneName=" + zoneName);
		}

		private static string FormatSelectedUrl(ContentItem selectedItem, string path)
		{
			Url url = new Url(path);
			if (selectedItem != null)
				url = url.AppendQuery("selected=" + selectedItem.Path);
			return url.ToString();
		}

		/// <summary>Gets the url to edit page creating new items.</summary>
		/// <param name="selected">The selected item.</param>
		/// <param name="definition">The type of item to edit.</param>
		/// <param name="zoneName">The zone to add the item to.</param>
		/// <param name="position">The position relative to the selected item to add the item.</param>
		/// <returns>The url to the edit page.</returns>
		public string GetEditNewPageUrl(ContentItem selected, ContentType definition, string zoneName, CreationPosition position)
		{
			if (selected == null) throw new ArgumentNullException("selected");
			if (definition == null) throw new ArgumentNullException("definition");

			ContentItem parent = (position != CreationPosition.Below) ? selected.Parent : selected;

			if (selected == null)
				throw new ZeusException("Cannot insert item before or after the root page.");

			Url url = new Url(EditItemUrl);
			url = url.AppendQuery("selected", parent.Path);
			url = url.AppendQuery("discriminator", definition.Discriminator);
			url = url.AppendQuery("zoneName", zoneName);

			switch (position)
			{
				case CreationPosition.Before:
					url = url.AppendQuery("before", selected.Path);
					break;
				case CreationPosition.After:
					url = url.AppendQuery("after", selected.Path);
					break;
			}

			return url.ToString();
		}

		public IEnumerable<ActionPluginGroupAttribute> GetActionPluginGroups()
		{
			return _cachedActionPluginGroups;
		}

		public string GetAdminDefaultUrl()
		{
			return Context.Current.GetServerResourceUrl(_adminAssembly.Assembly, "Zeus.Admin.Default.aspx");
		}

		/// <summary>Gets the url to the edit page where to edit an existing item in the original language.</summary>
		/// <param name="item">The item to edit.</param>
		/// <returns>The url to the edit page</returns>
		public string GetEditExistingItemUrl(ContentItem item)
		{
			return GetEditExistingItemUrl(item, item.Language);
		}

		/// <summary>Gets the url to the edit page where to edit an existing item.</summary>
		/// <param name="item">The item to edit.</param>
		/// <param name="language">The language to edit (or create if it doesn't exist).</param>
		/// <returns>The url to the edit page</returns>
		public string GetEditExistingItemUrl(ContentItem item, string languageCode)
		{
			if (item == null)
				return null;

			if (string.IsNullOrEmpty(languageCode))
				languageCode = item.Language;

			if (item.VersionOf != null)
				return string.Format("{0}?selectedUrl={1}&language={2}", EditItemUrl, HttpUtility.UrlEncode(item.FindPath(PathData.DefaultAction).RewrittenUrl), languageCode);

			return string.Format("{0}?selected={1}&language={2}", EditItemUrl, item.Path, languageCode);
		}

		public Func<IEnumerable<ContentItem>, IEnumerable<ContentItem>> GetEditorFilter(IPrincipal user)
		{
			return items => items.Authorized(user, _securityManager, Operations.Read).Pages();
		}

		/// <summary>Gets the url for the preview frame.</summary>
		/// <param name="selectedItem">The currently selected item.</param>
		/// <returns>An url.</returns>
		public string GetPreviewUrl(INode selectedItem)
		{
			string url = string.Format("{0}",
				selectedItem.PreviewUrl,
				HttpUtility.UrlEncode(selectedItem.PreviewUrl)
				);
			return Url.ToAbsolute(url);
		}

		/// <summary>Saves an item using values from the supplied item editor.</summary>
		/// <param name="item">The item to update.</param>
		/// <param name="addedEditors">The editors to update the item with.</param>
		/// <param name="versioningMode">How to treat the item beeing saved in respect to versioning.</param>
		/// <param name="user">The user that is performing the saving.</param>
		public virtual ContentItem Save(ContentItem item, IDictionary<string, Control> addedEditors, ItemEditorVersioningMode versioningMode, IPrincipal user,
			Action<ContentItem> onSavingCallback)
		{
			// when an unpublished version is saved and published
			if (versioningMode == ItemEditorVersioningMode.SaveAsMaster)
			{
				using (ITransaction tx = _persister.Repository.BeginTransaction())
				{
					ContentItem itemToUpdate = item.VersionOf;
					if (itemToUpdate == null) throw new ArgumentException("Expected the current item to be a version of another item.", "item");

					if (ShouldStoreVersion(item))
						SaveVersion(itemToUpdate);

					DateTime? published = itemToUpdate.Published;
					bool wasUpdated = UpdateItem(itemToUpdate, addedEditors, user);

					if (wasUpdated || IsNew(itemToUpdate))
					{
                        onSavingCallback(itemToUpdate);
                        foreach (ContentItem child in item.Children.ToList())
                        {
                            if (!child.HasMinRequirementsForSaving())
                                item.Children.Remove(child);
                        }
						itemToUpdate.Published = published ?? Utility.CurrentTime();
						_persister.Save(itemToUpdate);

                        ContentItem theParent = itemToUpdate.Parent;
                        while (theParent.Parent != null && (item.PropogateUpdate && theParent.PropogateUpdate))
                        { 
                            //go up the tree updating - if a child has been changed, so effectively has the parent
                            theParent.Updated = DateTime.Now;
                            _persister.Save(theParent);
                            theParent = theParent.Parent;
                        }
                    }

					tx.Commit();
					return item.VersionOf;
				}
			}

			// when an item is saved without any new version
			if (versioningMode == ItemEditorVersioningMode.SaveOnly)
			{
				bool wasUpdated = UpdateItem(item, addedEditors, user);
				if (wasUpdated || IsNew(item))
				{
					onSavingCallback(item);
                    foreach (ContentItem child in item.Children.ToList())
                    {
                        if (!child.HasMinRequirementsForSaving())
                            item.Children.Remove(child);
                    }
                    _persister.Save(item);

                    ContentItem theParent = item.Parent;
                    while (theParent.Parent != null && (item.PropogateUpdate && theParent.PropogateUpdate))
                    {
                        //go up the tree updating - if a child has been changed, so effectively has the parent
                        theParent.Updated = DateTime.Now;
                        _persister.Save(theParent);
                        theParent = theParent.Parent;
                    }
				}

				return item;
			}

			// when an item is saved but a version is stored before the item is updated
			if (versioningMode == ItemEditorVersioningMode.VersionAndSave)
			{
				using (ITransaction tx = _persister.Repository.BeginTransaction())
				{
					if (ShouldStoreVersion(item))
						SaveVersion(item);

					DateTime? initialPublished = item.Published;
					bool wasUpdated = UpdateItem(item, addedEditors, user);
					DateTime? updatedPublished = item.Published;

					// the item was the only version of an unpublished item - publish it
					if (initialPublished == null && updatedPublished == null)
					{
						item.Published = Utility.CurrentTime();
						wasUpdated = true;
					}

					IncrementVersion(item);

					if (wasUpdated || IsNew(item))
					{
						onSavingCallback(item);
                        _persister.Save(item);
                        tx.Commit();
					}

					return item;
				}
			}

			// when making a version without saving the item
			if (versioningMode == ItemEditorVersioningMode.VersionOnly)
			{
				using (ITransaction tx = _persister.Repository.BeginTransaction())
				{
					if (ShouldStoreVersion(item))
						item = SaveVersion(item);

					bool wasUpdated = UpdateItem(item, addedEditors, user);
					if (wasUpdated || IsNew(item))
					{
						onSavingCallback(item);
						item.Published = null;
						_persister.Save(item);
					}

					IncrementVersion(item);

					tx.Commit();
					return item;
				}
			}

			throw new ArgumentException("Unexpected versioning mode.", "versioningMode");
		}

		private void IncrementVersion(ContentItem item)
		{
			ContentItem masterItem = item.VersionOf ?? item;
			var versions = _versionManager.GetVersionsOf(masterItem);
			if (versions.Any())
			{
				int maxVersion = versions.Max(ci => ci.Version);
				item.Version = maxVersion + 1;
			}
		}

		/// <summary>Updates the item by way of letting the defined editable attributes interpret the added editors.</summary>
		/// <param name="item">The item to update.</param>
		/// <param name="addedEditors">The previously added editors.</param>
		/// <param name="user">The user for filtering updatable editors.</param>
		/// <returns>Whether any property on the item was updated.</returns>
		public bool UpdateItem(ContentItem item, IDictionary<string, Control> addedEditors, IPrincipal user)
		{
			if (item == null) throw new ArgumentNullException("item");
			if (addedEditors == null) throw new ArgumentNullException("addedEditors");

			bool updated = false;
			ContentType contentType = _contentTypeManager.GetContentType(item.GetType());
			foreach (IEditor e in contentType.GetEditors(user))
            {
				if (addedEditors.ContainsKey(e.Name))
                {
					updated = e.UpdateItem(item, addedEditors[e.Name]) || updated;
                    /*
                    if (updated)
                    {
                        System.Web.HttpContext.Current.Response.Write("Editor Name = " + e.Name + "<br/>");
                        System.Web.HttpContext.Current.Response.Write("Item Name = " + item.Name + "<br/>");
                        System.Web.HttpContext.Current.Response.Write("Item Url = " + item.Url + "<br/>");
                        System.Web.HttpContext.Current.Response.End();
                    }
                     */
                    
                }
            }
            

			return updated;
		}

		#region Helper methods

		private static bool IsNew(ContentItem current)
		{
			return current.ID == 0;
		}

		private bool ShouldStoreVersion(ContentItem item)
		{
			return EnableVersioning && !IsNew(item) && item.GetType().GetCustomAttributes(typeof(NotVersionableAttribute), true).Length == 0;
		}

		private ContentItem SaveVersion(ContentItem current)
		{
			ContentItem savedVersion = null;
			Utility.InvokeEvent(SavingVersion, current, this, delegate(ContentItem item)
			{
				savedVersion = _versionManager.SaveVersion(item);
			});
			return savedVersion;
		}

		#endregion

		#endregion
	}
}
