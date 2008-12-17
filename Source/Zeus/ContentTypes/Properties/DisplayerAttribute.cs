using System;
using System.Web.UI;
using Isis;

namespace Zeus.ContentTypes.Properties
{
	[AttributeUsage(AttributeTargets.Property)]
	public class DisplayerAttribute : Attribute, IDisplayer
	{
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

		public virtual Control AddTo(Control container, ContentItem item, string propertyName)
		{
			Control displayer = (Control) Activator.CreateInstance(this.ControlType);
			displayer.SetValue(this.ControlPropertyName, item[propertyName], false);
			container.Controls.Add(displayer);
			return displayer;
		}
	}
}
