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
		private Dictionary<Type, IEnumerable<Property>> _cachedFilteredProperties = new Dictionary<Type, IEnumerable<Property>>();

		#region Properties

		/// <summary>Gets or sets additional child types allowed below this item.</summary>
		public IList<ContentType> AllowedChildren
		{
			get { return _allowedChildren; }
		}

		public string Discriminator
		{
			get { return this.DefinitionAttribute.Name ?? this.ItemType.Name; }
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

		public ContentTypeAttribute DefinitionAttribute
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

		public IEnumerable<Property> EditableProperties
		{
			get { return GetPropertiesWithAttribute<IEditor>(); }
		}

		#endregion

		public ContentType(Type itemType)
		{
			this.ItemType = itemType;
			this.DefinitionAttribute = new ContentTypeAttribute { Title = itemType.Name, Name = itemType.Name };
		}

		#region Methods

		private IEnumerable<Property> GetPropertiesWithAttribute<T>()
			where T : class
		{
			Type type = typeof(T);
			if (!_cachedFilteredProperties.ContainsKey(type))
				_cachedFilteredProperties[type] = this.Properties.Where(p => p.UnderlyingProperty.GetCustomAttribute<T>(false, false) != null);
			return _cachedFilteredProperties[type];
		}

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
