using System;
using System.Diagnostics;

namespace Zeus.Web
{
	/// <summary>
	/// Used to bind a controller to a certain content type.
	/// </summary>
	[DebuggerDisplay("ControlsAttribute: {AdapterType}->{ItemType}")]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true)]
	public class ControlsAttribute : Attribute, IComparable<ControlsAttribute>, IAdapterDescriptor
	{
		private readonly Type itemType;

		public ControlsAttribute(Type itemType)
		{
			this.itemType = itemType;
		}

		/// <summary>The type of item beeing adapted.</summary>
		public Type ItemType
		{
			get { return itemType; }
		}

		/// <summary>The type of adapter referenced by this descriptor. This property is set by the framework as adapters are enumerated.</summary>
		public Type AdapterType { get; set; }

		/// <summary>The name of the controller. Used to reference the controller in ASP.NET MVC scenarios.</summary>
		public string ControllerName
		{
			get
			{
				string name = AdapterType.Name;
				int i = name.IndexOf("Controller");
				if (i > 0)
				{
					return name.Substring(0, i);
				}
				return name;
			}
		}

		public string AreaName { get; set; }

		/// <summary>Compares the path against the referenced item type to determine whether this is the correct adapter.</summary>
		/// <param name="path">The request path.</param>
		/// <param name="requiredType">The type of adapter needed.</param>
		/// <returns>True if the descriptor references the correct adapter.</returns>
		public bool IsAdapterFor(PathData path, Type requiredType)
		{
			if (path.IsEmpty())
				return false;

			return ItemType.IsAssignableFrom(path.CurrentItem.GetType()) && requiredType.IsAssignableFrom(AdapterType);
		}

		#region IComparable<IAdapterDescriptor> Members

		public int CompareTo(IAdapterDescriptor other)
		{
			return InheritanceDepth(other.ItemType) - InheritanceDepth(ItemType);
		}

		int InheritanceDepth(Type type)
		{
			if (type == null || type == typeof(object))
				return 0;
			return 1 + InheritanceDepth(type.BaseType);
		}

		#endregion

		#region IComparable<ControlsAttribute> Members

		int IComparable<ControlsAttribute>.CompareTo(ControlsAttribute other)
		{
			return CompareTo(other);
		}

		#endregion
	}
}