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
		private IList<IEditorContainer> _containers = new List<IEditorContainer>();

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

		public IEnumerable<Property> EditableProperties
		{
			get { return GetPropertiesWithAttribute<IEditor>().OrderBy(p => p.Editor.SortOrder); }
		}

		public IEnumerable<Property> DisplayableProperties
		{
			get { return GetPropertiesWithAttribute<IDisplayer>(); }
		}

		public IEditorContainer RootContainer
		{
			get;
			set;
		}

		public IList<IEditorContainer> Containers
		{
			get { return _containers; }
			set { _containers = value; }
		}

		#endregion

		public ContentType(Type itemType)
		{
			this.ItemType = itemType;
			this.ContentTypeAttribute = new ContentTypeAttribute { Title = itemType.Name, Name = itemType.Name };
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

		/// <summary>Adds an containable editor or container to existing editors and to a container.</summary>
		/// <param name="containable">The editable to add.</param>
		public void Add(IContainable containable)
		{
			if (string.IsNullOrEmpty(containable.ContainerName))
			{
				this.RootContainer.AddContained(containable);
				AddToCollection(containable);
			}
			else
			{
				foreach (IEditorContainer container in this.Containers)
				{
					if (container.Name == containable.ContainerName)
					{
						container.AddContained(containable);
						AddToCollection(containable);
						return;
					}
				}
				throw new ZeusException(
					"The editor '{0}' references a container '{1}' which doesn't seem to be defined on '{2}'. Either add a container with this name or remove the reference to that container.",
					containable.Name, containable.ContainerName, ItemType);
			}
		}

		private void AddToCollection(IContainable containable)
		{
			if (containable is IEditor)
				//this.Editables.Add(containable as IEditable);
				;
			else if (containable is IEditorContainer)
				this.Containers.Add(containable as IEditorContainer);
		}

		#endregion
	}
}
