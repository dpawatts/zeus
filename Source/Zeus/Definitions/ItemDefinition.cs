using System;
using System.Collections.Generic;
using Zeus.Edit;

namespace Zeus.Definitions
{
	public class ItemDefinition
	{
		private IList<ItemDefinition> _allowedChildren = new List<ItemDefinition>();

		#region Properties

		/// <summary>Gets or sets additional child types allowed below this item.</summary>
		public IList<ItemDefinition> AllowedChildren
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

		public DefinitionAttribute DefinitionAttribute
		{
			get;
			set;
		}

		public bool IsDefined
		{
			get;
			internal set;
		}

		public ICollection<IEditable> Editables
		{
			get;
			set;
		}

		#endregion

		public ItemDefinition(Type itemType)
		{
			this.ItemType = itemType;
			this.DefinitionAttribute = new DefinitionAttribute { Title = itemType.Name, Name = itemType.Name };
		}

		/// <summary>Adds an allowed child definition to the list of allowed definitions.</summary>
		/// <param name="definition">The allowed child definition to add.</param>
		public void AddAllowedChild(ItemDefinition definition)
		{
			if (!this.AllowedChildren.Contains(definition))
				this.AllowedChildren.Add(definition);
		}

		/// <summary>Removes an allowed child definition from the list of allowed definitions if not already removed.</summary>
		/// <param name="definition">The definition to remove.</param>
		public void RemoveAllowedChild(ItemDefinition definition)
		{
			if (this.AllowedChildren.Contains(definition))
				this.AllowedChildren.Remove(definition);
		}
	}
}
