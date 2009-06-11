using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Principal;
using System.Web.UI;
using Zeus.ContentTypes;
using Zeus.Globalization.ContentTypes;
using Zeus.Web.UI.WebControls;

namespace Zeus.Admin
{
	/// <summary>
	/// Classes implementing this interface can be used to interact with the 
	/// edit mode functionality.
	/// </summary>
	public interface IAdminManager
	{
		/// <summary>
		/// Gets or sets the language branch currently being edited.
		/// </summary>
		string CurrentAdminLanguageBranch { get; set; }

		IEnumerable<ActionPluginGroupAttribute> GetActionPluginGroups();

		IEnumerable<ActionPluginAttribute> GetActionPlugins(string groupName);

		/// <summary>Gets the url to the edit page where to edit an existing item in the original language.</summary>
		/// <param name="item">The item to edit.</param>
		/// <returns>The url to the edit page</returns>
		string GetEditExistingItemUrl(ContentItem item);

		/// <summary>Gets the url to the edit page where to edit an existing item.</summary>
		/// <param name="item">The item to edit.</param>
		/// <param name="languageCode">The translation to edit (or create if it doesn't exist).</param>
		/// <returns>The url to the edit page</returns>
		string GetEditExistingItemUrl(ContentItem item, string languageCode);

		/// <summary>Gets the url to edit page creating new items.</summary>
		/// <param name="selected">The selected item.</param>
		/// <param name="contentType">The type of item to edit.</param>
		/// <param name="zoneName">The zone to add the item to.</param>
		/// <param name="position">The position relative to the selected item to add the item.</param>
		/// <returns>The url to the edit page.</returns>
		string GetEditNewPageUrl(ContentItem selected, ContentType contentType, string zoneName, CreationPosition position);

		/// <summary>Gets the url to the select type of item to create.</summary>
		/// <param name="selectedItem">The currently selected item.</param>
		/// <returns>The url to the select new item to create page.</returns>
		string GetSelectNewItemUrl(ContentItem selectedItem);

		/// <summary>Gets the url to the select type of item to create.</summary>
		/// <param name="selectedItem">The currently selected item.</param>
		/// <param name="zoneName">The zone to select.</param>
		/// <returns>The url to the select new item to create page.</returns>
		string GetSelectNewItemUrl(ContentItem selectedItem, string zoneName);

		/// <summary>Gets the filter to be applied to items displayed in edit mode.</summary>
		/// <param name="user">The user for whom to apply the filter.</param>
		/// <returns>A filter.</returns>
		Func<IEnumerable<ContentItem>, IEnumerable<ContentItem>> GetEditorFilter(IPrincipal user);

		string GetEmbeddedResourceUrl(Assembly assembly, string resourcePath);

		/// <summary>Gets the url for the preview frame.</summary>
		/// <param name="selectedItem">The currently selected item.</param>
		/// <returns>A url.</returns>
		string GetPreviewUrl(INode selectedItem);

		/// <summary>Saves an item using values from the supplied item editor.</summary>
		/// <param name="item">The item to update.</param>
		/// <param name="addedEditors">The editors to update the item with.</param>
		/// <param name="versioningMode">How to treat the item beeing saved in respect to versioning.</param>
		/// <param name="user">The user that is performing the saving.</param>
		ContentItem Save(ContentItem item, IDictionary<string, Control> addedEditors, ItemEditorVersioningMode versioningMode, IPrincipal user);

		/// <summary>Updates the item with the values from the editors.</summary>
		/// <param name="item">The item to update.</param>
		/// <param name="addedEditors">The previously added editors.</param>
		/// <param name="user">The user for filtering updatable editors.</param>
		bool UpdateItem(ContentItem item, IDictionary<string, Control> addedEditors, IPrincipal user);

		bool TreeTooltipsEnabled { get; }
	}
}
