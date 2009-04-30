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
	}
}