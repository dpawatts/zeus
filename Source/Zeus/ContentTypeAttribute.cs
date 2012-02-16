using System;
using System.Collections.Generic;
using Zeus.ContentTypes;
using Zeus.Installation;

namespace Zeus
{
	/// <summary>
	/// Decoration for Zeus content items. Provides information needed in edit 
	/// mode and for data integrity.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class ContentTypeAttribute : AbstractContentTypeRefiner, IDefinitionRefiner
	{
		#region Public properties

		/// <summary>
		/// Gets or sets the name used when presenting this item type to editors.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the name/discriminator needed to map the appropriate type with content data 
		/// when retrieving from persistence. When this is null the type's full name is used.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the description of this item.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the order of this item type when selecting new item in edit mode.
		/// </summary>
		public int SortOrder { get; set; }

		/// <summary>
		/// Gets or sets the tooltip used when presenting this item type to editors.
		/// </summary>
		public string ToolTip { get; set; }

		/// <summary>
		/// Gets or sets how to treat this definition during installation.
		/// </summary>
		public InstallerHints Installer { get; set; }

		/// <summary>
		/// Gets or sets whether this type is treated as a page. This flag is used,
		/// for example, to show or hide the SEO tab.
		/// </summary>
		public bool IsPage { get; set; }

        /// <summary>
        /// Unfortunately the above also dicates behaviour in the admin system
        /// This will allow you to turn off the SEO Assets
        /// </summary>		
        public bool IgnoreSEOAssets { get; set; }

		#endregion

		#region Constructors

		public ContentTypeAttribute()
		{
			RefinementOrder = RefineOrder.First;
			IsPage = true;
            IgnoreSEOAssets = false;
		}

        /// <summary>Initializes a new instance of ContentTypeAttribute class.</summary>
        /// <param name="title">Sets IgnoreSEO to true.</param>
        public ContentTypeAttribute(bool ignoreSEOAssets)
            : this()
        {
            IgnoreSEOAssets = ignoreSEOAssets;
        }

		/// <summary>Initializes a new instance of ContentTypeAttribute class.</summary>
		/// <param name="title">The title used when presenting this item type to editors.</param>
		public ContentTypeAttribute(string title)
			: this()
		{
			Title = title;
		}

		/// <summary>Initializes a new instance of ContentTypeAttribute class.</summary>
		/// <param name="title">The title used when presenting this item type to editors.</param>
		/// <param name="name">The name/discriminator needed to map the appropriate type with content data when retrieving from persistence. When this is null the type's full name is used.</param>
		public ContentTypeAttribute(string title, string name)
			: this()
		{
			Title = title;
			Name = name;
		}

		/// <summary>Initializes a new instance of ContentTypeAttribute class.</summary>
		/// <param name="title">The title used when presenting this item type to editors.</param>
		/// <param name="name">The name/discriminator needed to map the appropriate type with content data when retrieving from persistence. When this is null the type's name is used.</param>
		/// <param name="description">The description of this item.</param>
		/// <param name="toolTip">The tool tip displayed when hovering over this item type.</param>
		/// <param name="sortOrder">The order of this type compared to other items types.</param>
		public ContentTypeAttribute(string title, string name, string description, string toolTip, int sortOrder)
			: this()
		{
			Title = title;
			Name = name;
			Description = description;
			ToolTip = toolTip;
			SortOrder = sortOrder;
		}

		#endregion

		public override void Refine(ContentType currentContentType, IList<ContentType> allContentTypes)
		{
			if (string.IsNullOrEmpty(this.Title))
				Title = currentContentType.ItemType.Name;

			currentContentType.ContentTypeAttribute = this;
			currentContentType.IsDefined = true;
			currentContentType.IsPage = IsPage;
            //currentContentType.IgnoreSEOAssets = IgnoreSEOAssets;
		}
	}
}
