using System;
using System.Collections.Generic;
using System.Linq;
using Zeus.ContentTypes.Properties;
using Isis.Reflection;

namespace Zeus.ContentTypes
{
	public class ContentType
	{
		private IList<ContentType> _allowedChildren = new List<ContentType>();

		#region Properties

		/// <summary>Gets or sets additional child types allowed below this item.</summary>
		public IList<ContentType> AllowedChildren
		{
			get { return _allowedChildren; }
		}

		public string Discriminator
		{
			get { return this.ContentTypeAttribute.Name ?? this.ItemType.Name; }
		}

		public IList<EditorContainerAttribute> EditorContainers
		{
			get;
			set;
		}

		public Type ItemType
		{
			get;
			set;
		}

		public string IconUrl
		{
			get { return ((ContentItem) Activator.CreateInstance(this.ItemType)).IconUrl; }
		}

		public ContentTypeAttribute ContentTypeAttribute
		{
			get;
			set;
		}

		public bool IsDefined
		{
			get;
			internal set;
		}

		public IEnumerable<Property> Properties
		{
			get;
			internal set;
		}

		public IEnumerable<IEditor> Editors
		{
			get;
			internal set;
		}

		public IEnumerable<IDisplayer> Displayers
		{
			get;
			internal set;
		}

		public IEditorContainer RootContainer
		{
			get;
			set;
		}

		public IEnumerable<IEditorContainer> Containers
		{
			get;
			internal set;
		}

		#endregion

		public ContentType(Type itemType)
		{
			this.ItemType = itemType;
			this.ContentTypeAttribute = new ContentTypeAttribute { Title = itemType.Name, Name = itemType.Name };
		}

		#region Methods

		/// <summary>Adds an allowed child definition to the list of allowed definitions.</summary>
		/// <param name="definition">The allowed child definition to add.</param>
		public void AddAllowedChild(ContentType definition)
		{
			if (!this.AllowedChildren.Contains(definition))
				this.AllowedChildren.Add(definition);
		}

		/// <summary>Removes an allowed child definition from the list of allowed definitions if not already removed.</summary>
		/// <param name="definition">The definition to remove.</param>
		public void RemoveAllowedChild(ContentType definition)
		{
			if (this.AllowedChildren.Contains(definition))
				this.AllowedChildren.Remove(definition);
		}

		#endregion
	}
}
