using System;
using System.Web.UI;
using Isis;

namespace Zeus.ContentTypes.Properties
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
			this.ControlType = controlType;
			this.ControlPropertyName = controlPropertyName;
		}

		#endregion

		#region Properties

		public string Name
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public Type ControlType
		{
			get;
			set;
		}

		public string ControlPropertyName
		{
			get;
			set;
		}

		#endregion

		public virtual void InstantiateIn(Control container)
		{
			_displayerControl = (Control) Activator.CreateInstance(this.ControlType);
			container.Controls.Add(_displayerControl);
		}

		public virtual void SetValue(Control container, ContentItem item, string propertyName)
		{
			_displayerControl.SetValue(this.ControlPropertyName, item[propertyName], false);
		}
	}
}
