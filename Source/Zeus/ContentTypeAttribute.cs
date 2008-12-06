using System;
using Zeus.ContentTypes;

namespace Zeus
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ContentTypeAttribute : Attribute, IDefinitionRefiner
	{
		#region Public properties

		public string Title
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public int SortOrder
		{
			get;
			set;
		}

		public string ToolTip
		{
			get;
			set;
		}

		#endregion

		#region Constructors

		public ContentTypeAttribute()
		{
		}

		/// <summary>Initializes a new instance of ItemAttribute class.</summary>
		/// <param name="title">The title used when presenting this item type to editors.</param>
		public ContentTypeAttribute(string title)
		{
			this.Title = title;
		}

		/// <summary>Initializes a new instance of ItemAttribute class.</summary>
		/// <param name="title">The title used when presenting this item type to editors.</param>
		/// <param name="name">The name/discriminator needed to map the appropriate type with content data when retrieving from persistence. When this is null the type's full name is used.</param>
		public ContentTypeAttribute(string title, string name)
		{
			this.Title = title;
			this.Name = name;
		}

		/// <summary>Initializes a new instance of ItemAttribute class.</summary>
		/// <param name="title">The title used when presenting this item type to editors.</param>
		/// <param name="name">The name/discriminator needed to map the appropriate type with content data when retrieving from persistence. When this is null the type's name is used.</param>
		/// <param name="description">The description of this item.</param>
		/// <param name="toolTip">The tool tip displayed when hovering over this item type.</param>
		/// <param name="sortOrder">The order of this type compared to other items types.</param>
		public ContentTypeAttribute(string title, string name, string description, string toolTip, int sortOrder)
		{
			this.Title = title;
			this.Name = name;
			this.Description = description;
			this.ToolTip = toolTip;
			this.SortOrder = sortOrder;
		}

		#endregion

		void IDefinitionRefiner.Refine(ContentType currentDefinition)
		{
			if (string.IsNullOrEmpty(this.Title))
				Title = currentDefinition.ItemType.Name;

			currentDefinition.DefinitionAttribute = this;
			currentDefinition.IsDefined = true;
		}
	}
}
