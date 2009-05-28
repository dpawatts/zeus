using System;
using System.Web.UI;
using Isis.ExtensionMethods;

namespace Zeus.Design.Displayers
{
	[AttributeUsage(AttributeTargets.Property)]
	public class DisplayerAttribute : Attribute, IDisplayer
	{
		private Control _displayerControl;

		#region Constructor

		public DisplayerAttribute()
		{
		}

		public DisplayerAttribute(Type controlType, string controlPropertyName)
		{
			ControlType = controlType;
			ControlPropertyName = controlPropertyName;
		}

		#endregion

		#region Properties

		public string Name { get; set; }

		public string Title { get; set; }

		public Type ControlType { get; set; }

		public string ControlPropertyName { get; set; }

		#endregion

		public virtual void InstantiateIn(Control container)
		{
			_displayerControl = (Control) Activator.CreateInstance(ControlType);
			container.Controls.Add(_displayerControl);
		}

		public virtual void SetValue(Control container, ContentItem item, string propertyName)
		{
			_displayerControl.SetValue(ControlPropertyName, item[propertyName], false);
		}

		/// <summary>Compares the sort order of editable attributes.</summary>
		public int CompareTo(IDisplayer other)
		{
			if (Title != null && other.Title != null)
				return Title.CompareTo(other.Title);
			if (Title != null)
				return -1;
			if (other.Title != null)
				return 1;
			return 0;
		}

		#region Equals & GetHashCode

		/// <summary>Checks another object for equality.</summary>
		/// <param name="obj">The other object to check.</param>
		/// <returns>True if the items are of the same type and have the same name.</returns>
		public override bool Equals(object obj)
		{
			DisplayerAttribute other = obj as DisplayerAttribute;
			if (other == null)
				return false;
			return (Name == other.Name);
		}

		/// <summary>Gets a hash code based on the attribute's name.</summary>
		/// <returns>A hash code.</returns>
		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}

		#endregion
	}
}